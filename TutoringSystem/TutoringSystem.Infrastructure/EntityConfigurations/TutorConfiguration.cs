using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Infrastructure.EntityConfigurations
{
    public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
    {
        public void Configure(EntityTypeBuilder<Tutor> builder)
        {
            builder.ToTable("Tutors");

            builder.HasMany(t => t.Students)
            .WithMany(s => s.Tutors)
            .UsingEntity<StudentTutor>(
                j => j
                    .HasOne(st => st.Student)
                    .WithMany(s => s.StudentTutors)
                    .HasForeignKey(st => st.StudentId),
                j => j
                    .HasOne(st => st.Tutor)
                    .WithMany(t => t.StudentTutors)
                    .HasForeignKey(st => st.TutorId));
        }
    }
}