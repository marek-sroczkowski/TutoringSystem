using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;
using TutoringSystem.Infrastructure.Repositories;
using TutoringSystem.Infrastructure.Validators;

namespace TutoringSystem.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddValidators();

            services.AddScoped<Seeder>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                })
                .AddFluentValidation();
            services.AddScoped<IValidator<RegisterStudentDto>, RegisterStudentValidation>();
            services.AddScoped<IValidator<RegisterTutorDto>, RegisterTutorValidation>();

            return services;
        }
    }
}
