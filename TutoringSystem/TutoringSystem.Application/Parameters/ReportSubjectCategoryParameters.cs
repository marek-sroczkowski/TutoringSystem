using System;
using System.ComponentModel.DataAnnotations;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Parameters
{
    public class ReportSubjectCategoryParameters
    {
        [Required]
        public SubjectCategory SubjectCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
