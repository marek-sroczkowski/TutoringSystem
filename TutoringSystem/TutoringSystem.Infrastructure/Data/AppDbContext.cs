using Microsoft.EntityFrameworkCore;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<AdditionalOrder> AdditionalOrders { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Tutor>().ToTable("Tutors");

            modelBuilder.Entity<Message>().HasOne(u => u.Sender)
                                     .WithMany(m => m.MessagesSent)
                                     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>().HasOne(u => u.Recipient)
                                     .WithMany(m => m.MessagesRecived)
                                     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Tutor).WithMany(t => t.Reservations)
                   .HasForeignKey(r => r.TutorId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
