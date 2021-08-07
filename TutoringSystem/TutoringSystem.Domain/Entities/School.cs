using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class School
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public int Year { get; set; }

        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
