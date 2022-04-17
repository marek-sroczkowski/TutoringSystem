using System;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Extensions;

namespace TutoringSystem.Domain.Entities
{
    public class ActivationToken : Entity
    {
        public string TokenContent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public ActivationToken()
        {
            IsActive = true;
            ExpirationDate = DateTime.Now.ToLocal().AddDays(1);
        }
    }
}
