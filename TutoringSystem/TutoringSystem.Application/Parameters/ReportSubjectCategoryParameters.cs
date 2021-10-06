using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Parameters
{
    public class ReportSubjectCategoryParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubjectCategory SubjectCategory { get; set; }
    }
}
