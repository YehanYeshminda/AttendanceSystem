using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAttendaceSys.Entities
{
    [Table("TblAttendace")]
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string EnrollNo { get; set; }
        
        [MaxLength(20)]
        public string RegNo { get; set; }
        public DateTime InTime { get; set; }
        public byte MachineFinger { get; set; }
        public string AddBy { get; set; }
        public DateTime AddOn { get; set; }
        public byte ManualAttendance { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }
        public byte Status { get; set; } = 0;
    }
}
