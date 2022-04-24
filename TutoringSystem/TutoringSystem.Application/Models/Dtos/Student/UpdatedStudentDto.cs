namespace TutoringSystem.Application.Models.Dtos.Student
{
    public class UpdatedStudentDto
    {
        public long StudentId { get; set; }
        public double HourRate { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
