using System;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Extensions;

namespace TutoringSystem.Domain.Entities
{
    public class PasswordResetCode : Entity
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public PasswordResetCode()
        {
            CreatedDate = DateTime.Now.ToLocal();
            IsActive = true;
        }
    }
}