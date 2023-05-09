using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendaceSys.Dtos
{
    public class UpdateEmployeeDto
    {
        [MaxLength(20)]
        public string RegNo { get; set; }
        [MaxLength(10)]
        public string EnrollNo { get; set; }
        [MaxLength(200)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Designation { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Status { get; set; } = 0;
    }
}
