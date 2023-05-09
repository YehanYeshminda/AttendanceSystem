using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAttendaceSys.Entities
{
    [Table("TblEmployee")]
    [Index(nameof(RegNo), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string RegNo { get; set; }
        [MaxLength(20)]
        public string EnrollNo { get; set; }
        [MaxLength(200)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Designation { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Status { get; set; } = 0;
        public string PictureUrl { get; set; }
    }
}
