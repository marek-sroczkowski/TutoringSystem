using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<AdditionalOrder> AdditionalOrders { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<ActivationToken> ActivationTokens { get; set; }
        public virtual DbSet<SingleReservation> SingleReservations { get; set; }
        public virtual DbSet<RecurringReservation> RecurringReservations { get; set; }
        public virtual DbSet<RepeatedReservation> RepeatedReservations { get; set; }
        public virtual DbSet<StudentTutor> StudentTutors { get; set; }
        public virtual DbSet<StudentTutorRequest> StudentTutorRequests { get; set; }
        public virtual DbSet<PushNotificationToken> PushNotificationTokens { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
