using System;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Extensions;

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
        public bool IsActive { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public AdditionalOrder()
        {
            ReceiptDate = DateTime.Now.ToLocal();
            Status = AdditionalOrderStatus.Pending;
            IsPaid = false;
            IsActive = true;
        }
    }
}
