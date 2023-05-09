using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeAttendaceSys.Extensions
{
    public static class ClaimsPricipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
            {
                return int.Parse(userIdClaim.Value);
            }

            throw new InvalidOperationException("User ID claim is missing or invalid.");
        }

        public static DateTime? GetAuthTime(this ClaimsPrincipal user)
        {
            string authTimeString = user.FindFirstValue(JwtRegisteredClaimNames.AuthTime);
            if (!string.IsNullOrEmpty(authTimeString) && DateTime.TryParse(authTimeString, out DateTime authTime))
            {
                return authTime;
            }

            return null;
        }
    }
}
