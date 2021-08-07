using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class Contact
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
