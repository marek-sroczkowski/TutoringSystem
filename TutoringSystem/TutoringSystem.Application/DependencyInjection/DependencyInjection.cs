using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Services;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();
            services.AddAutoMapper();
            services.AddAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdditionalOrderService, AdditionalOrderService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITutorService, TutorService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection("jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.JwtIssuer,
                    ValidAudience = jwtOptions.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
                };
            });

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, OrderResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, ReservationResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, SubjectResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, PhoneNumberResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, AddressResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, ContactResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, AvailabilityResourceOperationHandler>();

            return services;
        }
    }
}
