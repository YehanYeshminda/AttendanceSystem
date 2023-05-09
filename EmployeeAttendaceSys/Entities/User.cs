namespace EmployeeAttendaceSys.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte Status { get; set; } = 1;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
