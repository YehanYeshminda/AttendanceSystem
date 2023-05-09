using AutoMapper;
using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;

namespace EmployeeAttendaceSys.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
            CreateMap<CreateAttendanceDto, Attendance>();
            CreateMap<Attendance, GetAttendanceDto>();
        }
    }
}
