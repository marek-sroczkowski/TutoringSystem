using System;

namespace TutoringSystem.Domain.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        public long SenderId { get; set; }
        public virtual User Sender { get; set; }

        public long RecipientId { get; set; }
        public virtual User Recipient { get; set; }
    }
}
