using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Infrastructure.EntityConfigurations
{
    public class SingleReservationConfiguration : IEntityTypeConfiguration<SingleReservation>
    {
        public void Configure(EntityTypeBuilder<SingleReservation> builder)
        {
            builder.ToTable("SingleReservations");
        }
    }
}