namespace EmployeeAttendaceSys.Dtos
{
    public class SearchEmployeeByRegNoDto
    {
        public string RegNo { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
    }
}
