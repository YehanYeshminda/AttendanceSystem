using AutoMapper;
using EmployeeAttendaceSys.Data;
using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Extensions;
using EmployeeAttendaceSys.Interfaces;
using EmployeeAttendaceSys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace EmployeeAttendaceSys.Controllers
{
    public class AttendanceController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AttendanceController(DataContext context, IAttendanceRepository attendanceRepository, IEmployeeRepository employeeRepository, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult<Attendance>> CreateAttendace(CreateAttendanceDto createAttendanceDto)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            if (createAttendanceDto == null) return BadRequest("Missing attendance data");

            var attendance = new Attendance
            {
                EnrollNo = createAttendanceDto.EnrollNo,
                RegNo = createAttendanceDto.RegNo,
                InTime = createAttendanceDto.InTime,
                MachineFinger = 1,
                AddBy = User.GetUserId().ToString(),
                AddOn = DateTime.UtcNow,
                Status = 0,
                ManualAttendance = 1,
                Reason = createAttendanceDto.Reason
            };

            _attendanceRepository.AddAttendance(attendance);
            await _attendanceRepository.SaveAllAsync();

            return Ok(attendance);
        }

        [HttpPut("regNo")]
        public async Task<ActionResult<Attendance>> UpdateRegNo(CreateRegNoDto createRegNoDto)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();

            var attendancesWithEnrollNo = _attendanceRepository.GetAtendanceByEnrollNoAsync(createRegNoDto.EnrollNo);

            if (attendancesWithEnrollNo != null)
            {
                foreach (var attendance in attendancesWithEnrollNo)
                {
                    attendance.RegNo = createRegNoDto.RegNo;
                    _context.Entry(attendance).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("status")]
        public async Task<ActionResult<Attendance>> UpdateStatus(CreateStatusDto createStatusDto)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();

            var attendancesWithRegNo = await _attendanceRepository.GetAttendaceByRegNoAsync(createStatusDto.RegNo);

            if (attendancesWithRegNo != null)
            {
                foreach (var attendance in attendancesWithRegNo)
                {
                    attendance.Status = createStatusDto.Status;
                    _context.Entry(attendance).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAllAttendance()
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            return Ok(await _attendanceRepository.GetAttendaceAsync());
        }

        [HttpPost("date-based")]
        public async Task<ActionResult<IEnumerable<GetDailyAttendanceDto>>> GetAllAttendanceBasedOnDate(SearchAttendanceOnDate onDate)
        {
            if (!ValidateService.ValidateUser(User, _context))
                return Unauthorized();

            if (onDate == null || onDate.StartDate == default)
            {
                return BadRequest();
            }

            var date = onDate.StartDate.Date;

            var connectionString = _config.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DailyAttendance", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Date", date);

                command.ExecuteNonQuery();
            }

            var attendances = await _attendanceRepository.GetAttendanceByDateAsync(onDate.StartDate);
            var regnos = attendances.Select(a => a.RegNo).Distinct().ToList();
            var attendanceDtos = new List<GetDailyAttendanceDto>();

            foreach (var regno in regnos)
            {
                string imageUrl = await _employeeRepository.GetEmployeeImageUrlAsync(regno);
                var enrollNumber = await _attendanceRepository.GetEnrollNumberAsync(regno, onDate.StartDate);
                var inTimeDate = await _attendanceRepository.GetInTimeNumberAsync(regno, onDate.StartDate);
                var outTimeDate = await _attendanceRepository.GetOutTimeNumberAsync(regno, onDate.StartDate);
                var dateByRegNo = await _attendanceRepository.GetDateByRegNoAsync(regno);

                var attendanceDto = new GetDailyAttendanceDto   
                {
                    RegNo = regno,
                    PictureUrl = imageUrl,
                    EnrollNo = enrollNumber,
                    InTime = inTimeDate,
                    OutTime = outTimeDate,
                    Date = dateByRegNo
                };

                attendanceDtos.Add(attendanceDto);
            }

            return Ok(attendanceDtos);
        }

        [HttpPost("month-based")]
        public async Task<ActionResult<IEnumerable<GetMonthlyAttendanceDto>>> GetAllAttendanceBasedOnMonth(SearchAttendanceOnDate onMonth)
        {
            if (!ValidateService.ValidateUser(User, _context))
                return Unauthorized();

            if (onMonth == null || onMonth.StartDate == default || onMonth.EndDate == default)
            {
                return BadRequest();
            }

            var startDate = onMonth.StartDate.Date;
            var endDate = onMonth.EndDate.Date;
            var regNo = onMonth.RegNo;
            var nameUser = await _employeeRepository.GetSingleEmployeeByRegNo(onMonth.RegNo);
            var username = nameUser != null ? nameUser.Username : null;

            var connectionString = _config.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetAllDatesBetween", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                command.Parameters.AddWithValue("@RegNo", regNo);
                command.Parameters.AddWithValue("@Name", username);
                command.ExecuteNonQuery();
            }

            var attendances = await _attendanceRepository.GetAttendanceByMonthAsync(onMonth.StartDate, onMonth.EndDate);
            var attendanceDtos = new List<GetMonthlyAttendanceDto>();

            foreach (var item in attendances)
            {
                var imageUrl = await _employeeRepository.GetEmployeeImageUrlAsync(item.RegNo);

                var attendanceDto = new GetMonthlyAttendanceDto
                {
                    RegNo = item.RegNo,
                    PictureUrl = imageUrl,
                    InTime = item.InTime,
                    OutTime = item.OutTime,
                    UserName = item.Username,
                    Date = item.Date
                };
                attendanceDtos.Add(attendanceDto);
            }

            return Ok(attendanceDtos);
        }


        [HttpPost("upload-data")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest("No file was uploaded.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                List<FileData> dataPoints = new List<FileData>();

                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Trim().Split('\t');
                    if (values.Length == 6)
                    {
                        FileData dataPoint = new FileData();
                        dataPoint.RegNo = values[0];
                        dataPoint.InTime = DateTime.Parse(values[1]);
                        dataPoints.Add(dataPoint);
                    }
                }

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                };

                string json = JsonConvert.SerializeObject(dataPoints, settings);

                using (var stringReader = new StringReader(json))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    var serializer = new JsonSerializer();
                    var users = serializer.Deserialize<List<Attendance>>(jsonReader);

                    foreach (var user in users)
                    {
                        Console.WriteLine(user.RegNo);
                        user.EnrollNo = user.RegNo;
                        user.InTime = DateTime.Parse(user.InTime.ToString());
                        user.Status = 0;
                        user.AddOn = DateTime.Now;
                        user.ManualAttendance = 0;
                        user.MachineFinger = 0;
                        user.RegNo = null;
                        _context.Attendances.Add(user);
                    }
                }

                await _context.SaveChangesAsync();
            }

                return Ok("Data uploaded and processed successfully.");
            }
        }
}
