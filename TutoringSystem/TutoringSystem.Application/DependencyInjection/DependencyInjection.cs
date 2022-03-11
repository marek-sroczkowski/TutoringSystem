using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.ScheduleTasks;
using TutoringSystem.Application.Services;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Validators;
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
            services.AddScheduleTasks();
            services.AddValidators();
            services.AddSortHelpers();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdditionalOrderService, AdditionalOrderService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ISingleReservationService, SingleReservationService>();
            services.AddScoped<IRecurringReservationService, RecurringReservationService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITutorService, TutorService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IActivationTokenService, ActivationTokenService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPushNotificationTokenService, PushNotificationTokenService>();
            services.AddScoped<IStudentTutorRequestNotificationService, StudentTutorRequestNotificationService>();
            services.AddScoped<IStudentRequestService, StudentRequestService>();
            services.AddScoped<IRepeatedReservationService, RepeatedReservationService>();

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
            services.AddScoped<IAuthorizationHandler, AvailabilityResourceOperationHandler>();
            services.AddScoped<IAuthorizationHandler, StudentResourceOperationHandler>();

            return services;
        }

        public static IServiceCollection AddScheduleTasks(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, RecurringReservationSynchronization>();

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

            services.AddScoped<IValidator<NewStudentDto>, NewStudentValidation>();
            services.AddScoped<IValidator<RegisteredTutorDto>, RegisteredTutorValidation>();
            services.AddScoped<IValidator<NewSubjectDto>, SubjectCreationValidation>();
            services.AddScoped<IValidator<UpdatedSubjectDto>, SubjectEditionValidation>();
            services.AddScoped<IValidator<RegisteredStudentDto>, RegisteredStudentValidation>();

            return services;
        }

        public static IServiceCollection AddSortHelpers(this IServiceCollection services)
        {
            services.AddScoped<ISortHelper<AdditionalOrder>, SortHelper<AdditionalOrder>>();
            services.AddScoped<ISortHelper<StudentReportDto>, SortHelper<StudentReportDto>>();
            services.AddScoped<ISortHelper<SubjectReportDto>, SortHelper<SubjectReportDto>>();
            services.AddScoped<ISortHelper<PlaceReportDto>, SortHelper<PlaceReportDto>>();
            services.AddScoped<ISortHelper<SubjectCategoryReportDto>, SortHelper<SubjectCategoryReportDto>>();
            services.AddScoped<ISortHelper<Reservation>, SortHelper<Reservation>>();

            return services;
        }
    }
}