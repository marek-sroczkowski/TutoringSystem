namespace TutoringSystem.Domain.Entities
{
    public class StudentTutor
    {
        public long StudentId { get; set; }
        public virtual Student Student { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public double HourlRate { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }

        public StudentTutor()
        {
            IsActive = true;
        }

        public StudentTutor(long studentId, long tutorId, double hourlRate, string note) : this()
        {
            StudentId = studentId;
            TutorId = tutorId;
            HourlRate = hourlRate;
            Note = note;
        }
    }
}
