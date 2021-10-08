using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class AdditionalOrder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public AdditionalOrderStatus Status { get; set; }
        public bool IsActiv { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public AdditionalOrder()
        {
            ReceiptDate = DateTime.Now;
            Status = AdditionalOrderStatus.Pending;
            IsPaid = false;
        }
    }
}
