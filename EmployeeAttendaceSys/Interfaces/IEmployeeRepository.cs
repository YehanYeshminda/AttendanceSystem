using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;

namespace EmployeeAttendaceSys.Interfaces
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void Update(Employee user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<string> GetEmployeeImageUrlAsync(string url);
        Task<Employee> GetEmployeeByUsernameAsync(string username);
        Task<IEnumerable<EmployeeDto>> GetEmployeesDtoAsync();
        Task<EmployeeDto> GetEmployeesDtoByIdAsync(int id);
        Task<Employee> GetEmployeeByRegNoAsync(string regNo);
        Task<IEnumerable<Attendance>> GetAttendanceByRegNo(string regNo);
        Task<Employee> GetSingleEmployeeByRegNo(string regNo);
    }
}
