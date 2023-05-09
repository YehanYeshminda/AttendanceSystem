using EmployeeAttendaceSys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data;

namespace EmployeeAttendaceSys.Controllers
{
    public class MonthlyAttendanceController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public MonthlyAttendanceController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult GetMonthlyAttendance([FromBody] JObject requestData)
        {
            var startDate = requestData["startDate"].Value<DateTime>();
            var endDate = requestData["endDate"].Value<DateTime>();
            var regNo = requestData["regNo"].Value<string>();
            var username = requestData["Username"].Value<string>();

            var connectionString = _config.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetAllDatesBetween", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                command.Parameters.AddWithValue("@RegNo", regNo);
                command.Parameters.AddWithValue("@Name", username);
                command.ExecuteNonQuery();
                return Ok();
            }
        }
    }
}
