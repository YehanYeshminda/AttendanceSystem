namespace EmployeeAttendaceSys.Dtos
{
    public class GetMonthlyAttendanceDto
    {
        public string RegNo { get; set; }
        public string UserName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string PictureUrl { get; set; }
        public string EnrollNo { get; set; }
    }
}
