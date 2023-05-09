using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendaceSys.Data
{
    public class DailyAttendanceRepository : IDailyAttendance
    {
        private readonly DataContext _context;

        public DailyAttendanceRepository(DataContext context)
        {
            _context = context;
        }
        public void AddDailyAttendance(DailyAttendance dailyAttendance)
        {
            _context.DailyAttendances.Add(dailyAttendance);
        }

        public async Task<IEnumerable<DailyAttendance>> GetDailyAttendanceAsync()
        {
            return await _context.DailyAttendances.ToListAsync();
        }

        public void RemoveDailyAttendance(DailyAttendance dailyAttendance)
        {
            _context.DailyAttendances.Remove(dailyAttendance);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(DailyAttendance dailyAttendance)
        {
            _context.Entry(dailyAttendance).State = EntityState.Modified;
        }
    }
}
