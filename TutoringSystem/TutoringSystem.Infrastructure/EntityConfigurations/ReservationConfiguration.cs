using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Infrastructure.EntityConfigurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasOne(r => r.Tutor).WithMany(t => t.Reservations)
               .HasForeignKey(r => r.TutorId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}