using System;

namespace TutoringSystem.Domain.Entities
{
    public class StudentTutorRequest
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsActive { get; set; }

        public long StudentId { get; set; }
        public virtual Student Student { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public StudentTutorRequest()
        {
            CreatedDate = DateTime.Now;
            IsAccepted = false;
            IsActive = true;
        }
    }
}
