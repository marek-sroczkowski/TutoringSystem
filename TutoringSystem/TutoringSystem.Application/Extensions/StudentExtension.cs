using System.Linq;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Extensions
{
    public static class StudentExtension
    {
        public static double GetHourRate(this Student student, long tutorId)
        {
            if (student.StudentTutors is null)
                return -1;

            var studentTutor = student.StudentTutors.FirstOrDefault(st => st.TutorId.Equals(tutorId));

            return studentTutor is null ? -1 : studentTutor.HourlRate;
        }
    }
}
