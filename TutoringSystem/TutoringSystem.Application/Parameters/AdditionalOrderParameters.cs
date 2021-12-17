using System;
using System.ComponentModel.DataAnnotations;

namespace TutoringSystem.Application.Parameters
{
    public class AdditionalOrderParameters : QueryStringParameters
    {
        [Required]
        public bool IsPaid { get; set; }

        [Required]
        public bool IsNotPaid { get; set; }

        [Required]
        public bool IsInProgress { get; set; }

        [Required]
        public bool IsPending { get; set; }

        [Required]
        public bool IsRealized { get; set; }

        [Required]
        public DateTime ReceiptStartDate { get; set; }

        [Required]
        public DateTime ReceiptEndDate { get; set; }

        [Required]
        public DateTime DeadlineStart { get; set; }

        [Required]
        public DateTime DeadlineEnd { get; set; }
    }
}