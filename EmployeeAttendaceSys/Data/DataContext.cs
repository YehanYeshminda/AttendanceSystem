using EmployeeAttendaceSys.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendaceSys.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> AdminUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<DailyAttendance> DailyAttendances { get; set; }
        public DbSet<MonthlyAttendance> MonthlyAttendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyAttendance>()
               .Property(d => d.Date)
               .HasColumnType("date");

            modelBuilder.Entity<MonthlyAttendance>()
                .Property(d => d.Date)
                .HasColumnType("date");
        }

    }
}
