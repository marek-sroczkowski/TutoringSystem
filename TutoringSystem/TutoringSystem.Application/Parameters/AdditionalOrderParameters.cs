using System;

namespace TutoringSystem.Application.Parameters
{
    public class AdditionalOrderParameters : QueryStringParameters
    {
        public bool IsPaid { get; set; }
        public bool IsNotPaid { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsPending { get; set; }
        public bool IsRealized { get; set; }
        public DateTime ReceiptStartDate { get; set; }
        public DateTime ReceiptEndDate { get; set; }
        public DateTime DeadlineStart { get; set; }
        public DateTime DeadlineEnd { get; set; }
    }
}
