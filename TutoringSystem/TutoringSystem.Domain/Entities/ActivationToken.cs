using System;

namespace TutoringSystem.Domain.Entities
{
    public class ActivationToken
    {
        public long Id { get; set; }
        public string TokenContent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public ActivationToken()
        {
            IsActive = true;
            ExpirationDate = DateTime.Now.AddDays(1);
        }
    }
}
