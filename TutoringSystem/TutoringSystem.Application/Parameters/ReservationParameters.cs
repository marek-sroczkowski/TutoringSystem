using System;
using System.ComponentModel.DataAnnotations;

namespace TutoringSystem.Application.Parameters
{
    public class ReservationParameters : QueryStringParameters
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsAtTutor { get; set; }

        [Required]
        public bool IsAtStudent { get; set; }

        [Required]
        public bool IsOnline { get; set; }
    }
}