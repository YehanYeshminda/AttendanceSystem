using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAttendaceSys.Services
{
    public static class ValidateService
    {
        public static bool ValidateUser(ClaimsPrincipal user, DbContext context)
        {
            if (user == null)
            {
                return false;
            }

            var getUserId = user.GetUserId();
            if (context.Set<User>().Find(getUserId) == null)
            {
                return false;
            }

            var getUserName = user.GetUsername();
            if (context.Set<User>().SingleOrDefault(x => x.UserName == getUserName) == null)
            {
                return false;
            }

            return true;
        }
    }
}
