using EmployeeAttendaceSys.Entities;

namespace EmployeeAttendaceSys.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
