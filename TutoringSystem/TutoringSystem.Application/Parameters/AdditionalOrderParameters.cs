using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Parameters
{
    public class AdditionalOrderParameters : QueryStringParameters
    {
        public bool? IsPaid { get; set; }
        public AdditionalOrderStatus? Status { get; set; }
        public DateTime? ReceiptStartDate { get; set; }
        public DateTime? ReceiptEndDate { get; set; }
        public DateTime? DeadlineStart { get; set; }
        public DateTime? DeadlineEnd { get; set; }
    }
}
