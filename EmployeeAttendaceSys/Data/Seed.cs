using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using Newtonsoft.Json;

namespace EmployeeAttendaceSys.Data
{
    public class Seed
    {
        private readonly IConfiguration _config;

        public Seed(IConfiguration config)
        {
            _config = config;
        }
        public static async Task ConvertDataToJson(DataContext context)
        {
            string filePath = "Data/fingerprintdata.txt";

            List<FileData> dataPoints = new List<FileData>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] values = line.Trim().Split('\t');
                if (values.Length == 6)
                {
                    FileData dataPoint = new FileData();
                    dataPoint.RegNo = values[0];
                    dataPoint.InTime = DateTime.Parse(values[1]);
                    dataPoints.Add(dataPoint);
                }
            }

            List<Attendance> usersAttendance = new List<Attendance>();

            if (!context.Attendances.Any())
            {
                foreach (var dataPoint in dataPoints)
                {
                    Attendance userAttendance = new Attendance
                    {
                        EnrollNo = dataPoint.RegNo,
                        InTime = dataPoint.InTime,
                        Status = 0,
                        MachineFinger = 0
                    };

                    usersAttendance.Add(userAttendance);
                }
            }

            context.Attendances.AddRange(usersAttendance);
            await context.SaveChangesAsync();

        }
    }
}
