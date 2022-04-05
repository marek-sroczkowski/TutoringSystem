namespace TutoringSystem.Domain.Entities
{
    public class Device
    {
        public long Id { get; set; }
        public string Identificator { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }
    }
}