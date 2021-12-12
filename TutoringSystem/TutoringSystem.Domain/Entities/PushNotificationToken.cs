using System;

namespace TutoringSystem.Domain.Entities
{
    public class PushNotificationToken
    {
        public long Id { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Token { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}