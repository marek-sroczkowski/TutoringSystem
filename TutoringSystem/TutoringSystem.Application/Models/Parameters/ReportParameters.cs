using System;
using System.ComponentModel.DataAnnotations;

namespace TutoringSystem.Application.Models.Parameters
{
    public class ReportParameters
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string OrderBy { get; set; }

        public ReportParameters()
        {
        }

        public ReportParameters(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public ReportParameters(DateTime startDate, DateTime endDate, string orderBy) : this(startDate, endDate)
        {
            OrderBy = orderBy;
        }
    }
}