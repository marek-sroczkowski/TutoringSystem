using System;
using TutoringSystem.Domain.Extensions;

namespace TutoringSystem.Domain.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByIp { get; set; }
        public string DeviceIdentificator { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string RevokedByIp { get; set; }
        public string RevokedByDeviceId { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }

        public RefreshToken()
        {
            CreatedDate = DateTime.Now.ToLocal();
            IsActive = true;
        }
    }
}