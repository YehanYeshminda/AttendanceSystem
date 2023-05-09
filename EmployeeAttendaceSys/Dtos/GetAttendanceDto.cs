using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendaceSys.Dtos
{
    public class GetAttendanceDto
    {
        public string EnrollNo { get; set; }
        public string RegNo { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public byte MachineFinger { get; set; }
        public string AddBy { get; set; }
        public DateTime AddOn { get; set; }
        public byte Status { get; set; }
        public string PictureUrl { get; set; }
    }
}
