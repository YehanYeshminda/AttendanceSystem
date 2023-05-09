using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendaceSys.Dtos
{
    public class CreateAttendanceDto
    {
        public string EnrollNo { get; set; }

        [MaxLength(20)]
        public string RegNo { get; set; }
        public DateTime InTime { get; set; }
        public byte MachineFinger { get; set; }
        public string AddBy { get; set; }
        public DateTime AddOn { get; set; }
        public byte Status { get; set; } = 0;
        public string ManualAttendance { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }
    }
}
