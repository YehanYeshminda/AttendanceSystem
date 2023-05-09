namespace EmployeeAttendaceSys.Dtos
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string EncryptedPassword { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
