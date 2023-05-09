using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAttendaceSys.Entities
{
    [Table("TblDailyAttendance")]
    public class DailyAttendance
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string RegNo { get; set; }
        [MaxLength(200)]
        public string Username { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
    }
}
