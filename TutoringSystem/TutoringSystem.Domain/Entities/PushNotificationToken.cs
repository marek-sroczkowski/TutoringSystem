using System;
using TutoringSystem.Domain.Entities.Base;

namespace TutoringSystem.Domain.Entities
{
    public class PushNotificationToken : Entity
    {
        public DateTime ModificationDate { get; set; }
        public string Token { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}