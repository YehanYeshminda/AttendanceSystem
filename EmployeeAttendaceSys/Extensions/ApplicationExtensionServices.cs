using EmployeeAttendaceSys.Data;
using EmployeeAttendaceSys.Interfaces;
using EmployeeAttendaceSys.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendaceSys.Extensions
{
    public static class ApplicationExtensionServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IDailyAttendance, DailyAttendanceRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
