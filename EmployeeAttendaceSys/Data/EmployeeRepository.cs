using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendaceSys.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeByRegNoAsync(string regNo)
        {

            return await _context.Employees.SingleOrDefaultAsync(x => x.RegNo == regNo);
        }

        public async Task<Employee> GetEmployeeByUsernameAsync(string username)
        {
            return await _context.Employees
                .Where(x => x.Username == username)
                .SingleOrDefaultAsync();
        }

        public async Task<string> GetEmployeeImageUrlAsync(string regNo)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.RegNo == regNo);
            return employee?.PictureUrl;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByRegNo(string regNo)
        {
            return await _context.Employees.Where(e => e.RegNo == regNo).ToListAsync();
        }


        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.OrderBy(x => x.RegNo).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesDtoAsync()
        {
            return await _context.Employees
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EmployeeDto> GetEmployeesDtoByIdAsync(int id)
        {
            return await _context.Employees.Where(x => x.Id == id)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public void RemoveEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Employee user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByRegNo(string regNo)
        {
            var employees = await _context.Attendances.Where(e => e.RegNo == regNo).ToListAsync();
            return employees;
        }

        public async Task<Employee> GetSingleEmployeeByRegNo(string regNo)
        {
            var employees = await _context.Employees.SingleOrDefaultAsync(x => x.RegNo == regNo);
            return employees;
        }
    }
}
