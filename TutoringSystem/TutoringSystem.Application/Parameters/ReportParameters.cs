using System;
using System.ComponentModel.DataAnnotations;

namespace TutoringSystem.Application.Parameters
{
    public class ReportParameters
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string OrderBy { get; set; }
    }
}