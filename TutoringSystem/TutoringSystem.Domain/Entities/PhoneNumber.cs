namespace TutoringSystem.Domain.Entities
{
    public class PhoneNumber
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }
        public bool IsActiv { get; set; }

        public long ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public PhoneNumber()
        {
            IsActiv = true;
        }
    }
}
