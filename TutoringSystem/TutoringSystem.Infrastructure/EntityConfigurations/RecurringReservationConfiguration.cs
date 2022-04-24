using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Infrastructure.EntityConfigurations
{
    public class RecurringReservationConfiguration : IEntityTypeConfiguration<RecurringReservation>
    {
        public void Configure(EntityTypeBuilder<RecurringReservation> builder)
        {
            builder.ToTable("RecurringReservations");
        }
    }
}