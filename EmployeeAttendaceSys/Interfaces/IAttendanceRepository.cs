using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;

namespace EmployeeAttendaceSys.Interfaces
{
    public interface IAttendanceRepository
    {
        void AddAttendance(Attendance attendance);
        void RemoveAttendance(Attendance attendance);
        void Update(Attendance attendance);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Attendance>> GetAttendaceAsync();
        //Attendance GetAtendanceByIdAsync(string id);
        IEnumerable<Attendance> GetAtendanceByEnrollNoAsync(string id);
        Task<IEnumerable<Attendance>> GetAttendaceByRegNoAsync(string id);
        Task<IEnumerable<Attendance>> GetAttendaceByRegNoToListAsync(SearchEmployeeByRegNoDto byRegNoDto);
        Task<IEnumerable<DailyAttendance>> GetAttendanceByDateAsync(DateTime searchDate);
        Task<IEnumerable<MonthlyAttendance>> GetAttendanceByMonthAsync(DateTime startDate, DateTime endDate);
        Task<string> GetEnrollNumberAsync(string regNo, DateTime date);
        Task<DateTime> GetInTimeNumberAsync(string regNo, DateTime date);
        Task<DateTime> GetOutTimeNumberAsync(string regNo, DateTime date);
        Task<DateTime> GetDateByRegNoAsync(string regNo);
    }
}
