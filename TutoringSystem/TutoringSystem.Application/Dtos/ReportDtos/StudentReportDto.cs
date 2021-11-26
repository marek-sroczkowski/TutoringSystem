using TutoringSystem.Application.Dtos.StudentDtos;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class StudentReportDto
    {
        public StudentDto Student { get; set; }
        public double Hours { get; set; }
        public double Profit { get; set; }
    }
}
