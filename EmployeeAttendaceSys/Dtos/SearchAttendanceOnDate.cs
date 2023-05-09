namespace EmployeeAttendaceSys.Dtos
{
    public class SearchAttendanceOnDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RegNo { get; set; }
    }
}
