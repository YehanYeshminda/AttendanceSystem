using AutoMapper;
using EmployeeAttendaceSys.Data;
using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Interfaces;
using EmployeeAttendaceSys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendaceSys.Controllers
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAttendanceRepository _attendanceRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper, DataContext context, IWebHostEnvironment hostEnvironment, IAttendanceRepository attendanceRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _context = context;
            _hostEnvironment = hostEnvironment;
            _attendanceRepository = attendanceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            return Ok(await _employeeRepository.GetEmployeesAsync());
        }

        [HttpGet("{regNo}")]
        public async Task<ActionResult<Employee>> GetEmployeesByRegNo(string regNo)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            return Ok(await _employeeRepository.GetEmployeeByRegNoAsync(regNo));
        }

        [HttpGet("enroll/{enrollNo}")]
        public async Task<ActionResult<Employee>> GetEmployeeEnrollNoByRegNo(string regNo)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            return Ok(await _employeeRepository.GetEmployeeByRegNoAsync(regNo));
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromForm] CreateEmployeeDto createEmployeeDto)
        {
            if (!ValidateService.ValidateUser(User, _context))
            {
                return Unauthorized();
            }

            if (createEmployeeDto == null)
            {
                return BadRequest("Missing employee data");
            }

            var employeesAlreadyExisting = await _employeeRepository.GetEmployeeByRegNoAsync(createEmployeeDto.RegNo);
            if (employeesAlreadyExisting != null)
            {
                return BadRequest("Employee with Registration Number Already Exists");
            }   

            var employee = _mapper.Map<Employee>(createEmployeeDto);

            if (createEmployeeDto.File != null && createEmployeeDto.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(createEmployeeDto.File.FileName)}";
                string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createEmployeeDto.File.CopyToAsync(fileStream);
                }

                employee.PictureUrl = fileName;
            }

            _employeeRepository.AddEmployee(employee);
            await _employeeRepository.SaveAllAsync();

            return Ok(employee);
        }

        [AllowAnonymous]
        [HttpGet("images/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            string apiFolder = _hostEnvironment.ContentRootPath;
            string filePath = Path.Combine(apiFolder, "images", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileInfo = new FileInfo(filePath); 
                var mimeType = "application/octet-stream";

                return File(fileStream, mimeType, fileInfo.Name);
            }

            return NotFound();
        }


        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ValidateService.ValidateUser(User, _context)) return Unauthorized();
            if (updateEmployeeDto == null) return BadRequest("Missing employee data");

            var employee = await _employeeRepository.GetEmployeeByRegNoAsync(updateEmployeeDto.RegNo);

            if (employee == null)
            {
                return BadRequest("Employee with this regNo does not exist");
            }

            _mapper.Map(updateEmployeeDto, employee);
            await _employeeRepository.SaveAllAsync();   

            return NoContent();
        }

        [HttpPost("searchRegno")]
        public async Task<ActionResult<IEnumerable<GetAttendanceDto>>> GetAttendanceByRegNo(SearchEmployeeByRegNoDto byRegNoDto)
        {
            if (!ValidateService.ValidateUser(User, _context))
                return Unauthorized();

            var attendanceList = await _attendanceRepository.GetAttendaceByRegNoToListAsync(byRegNoDto);

            var attendanceDtos = new List<GetAttendanceDto>();

            foreach (var item in attendanceList)
            {
                string imageUrl = await _employeeRepository.GetEmployeeImageUrlAsync(item.RegNo);
                var outTimeDate = await _attendanceRepository.GetOutTimeNumberAsync(item.RegNo, byRegNoDto.InTime);


                var attendanceDto = new GetAttendanceDto
                {
                    RegNo = item.RegNo,
                    PictureUrl = imageUrl,
                    EnrollNo = item.EnrollNo,
                    InTime = item.InTime,
                    OutTime = outTimeDate
                };

                attendanceDtos.Add(attendanceDto);
            }

            return Ok(attendanceDtos);
        }
    }
}
