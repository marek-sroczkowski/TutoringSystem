using System;
using System.ComponentModel.DataAnnotations;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Parameters
{
    public class ReportPlaceParameters
    {
        [Required]
        public ReservationPlace Place { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
