using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;
using TutoringSystem.Infrastructure.Repositories;

namespace TutoringSystem.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
            services.AddDbContext(configuration);

            services.AddScoped<Seeder>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();
            services.AddScoped<IAdditionalOrderRepository, AdditionalOrderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ISingleReservationRepository, SingleReservationRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
            services.AddScoped<IIntervalRepository, IntervalRepository>();
            services.AddScoped<IActivationTokenRepository, ActivationTokenRepository>();
            services.AddScoped<IRecurringReservationRepository, RecurringReservationRepository>();
            services.AddScoped<IRepeatedReservationRepository, RepeatedReservationRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IStudentTutorRepository, StudentTutorRepository>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
