using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendaceSys.Data
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DataContext _context;

        public AttendanceRepository(DataContext context)
        {
            _context = context;
        }
        public void AddAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
        }

        public IEnumerable<Attendance> GetAtendanceByEnrollNoAsync(string id)
        {
            return _context.Attendances.Where(x => x.EnrollNo == id).ToList();
        }

        public async Task<IEnumerable<Attendance>> GetAttendaceByRegNoAsync(string id)
        {
            return await _context.Attendances.
                Where(x => x.RegNo == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<MonthlyAttendance>> GetAttendanceByMonthAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MonthlyAttendances
                .Where(x => x.InTime.HasValue && x.InTime.Value.Date >= startDate && x.InTime.Value.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DailyAttendance>> GetAttendanceByDateAsync(DateTime searchDate)
        {
            return await _context.DailyAttendances
                .Where(x => x.InTime.HasValue && x.InTime.Value.Date == searchDate.Date)
                .ToListAsync();
        }


        public async Task<string> GetEnrollNumberAsync(string regNo, DateTime date)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.RegNo == regNo && a.InTime.Date == date.Date);

            if (attendance != null)
            {
                return attendance.EnrollNo;
            }
            else
            {
                return null;
            }
        }

        public async Task<DateTime> GetInTimeNumberAsync(string regNo, DateTime date)
        {
            var employees = await _context.DailyAttendances
                .FirstOrDefaultAsync(a => a.RegNo == regNo && a.InTime.HasValue && a.InTime.Value.Date == date);

            if (employees != null)
            {
                return (DateTime)employees.InTime;
            }
            else
            {
                return new DateTime(2001,01,01);
            }
        }

        public async Task<IEnumerable<Attendance>> GetAttendaceAsync()
        {
            return await _context.Attendances.ToListAsync();
        }

        public void RemoveAttendance(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Attendance attendance)
        {
            _context.Entry(attendance).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Attendance>> GetAttendaceByRegNoToListAsync(SearchEmployeeByRegNoDto byRegNoDto)
        {
            return await _context.Attendances.
                Where(x => x.RegNo == byRegNoDto.RegNo && x.InTime >= byRegNoDto.InTime && x.InTime <= byRegNoDto.OutTime)
                .ToListAsync();
        }

        public async Task<DateTime> GetOutTimeNumberAsync(string regNo, DateTime date)
        {
            var employees = await _context.DailyAttendances
                .FirstOrDefaultAsync(a => a.RegNo == regNo && a.OutTime.HasValue && a.OutTime.Value.Date == date);

            if (employees != null)
            {
                return (DateTime)employees.OutTime;
            }
            else
            {
                return new DateTime(2001, 01, 01);
            }
        }

        public async Task<DateTime> GetDateByRegNoAsync(string regNo)
        {
            var employees = await _context.DailyAttendances
                .FirstOrDefaultAsync(a => a.RegNo == regNo);

            if (employees != null)
            {
                return (DateTime)employees.Date;
            }
            else
            {
                return new DateTime(2001, 01, 01);
            }
        }
    }
}
