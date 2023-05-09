using EmployeeAttendaceSys.Entities;

namespace EmployeeAttendaceSys.Interfaces
{
    public interface IDailyAttendance
    {
        void AddDailyAttendance(DailyAttendance dailyAttendance);
        void RemoveDailyAttendance(DailyAttendance dailyAttendance);
        void Update(DailyAttendance dailyAttendance);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<DailyAttendance>> GetDailyAttendanceAsync();
    }
}
