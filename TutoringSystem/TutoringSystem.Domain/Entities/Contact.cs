using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Base;

namespace TutoringSystem.Domain.Entities
{
    public class Contact : Entity
    {
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
