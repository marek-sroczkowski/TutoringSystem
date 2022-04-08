using TutoringSystem.Domain.Entities.Base;

namespace TutoringSystem.Domain.Entities
{
    public class PhoneNumber : Entity
    {
        public string Owner { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }

        public long ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public PhoneNumber()
        {
            IsActive = true;
        }
    }
}
