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
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Password;
using TutoringSystem.Application.Models.Dtos.Report;
using TutoringSystem.Application.Models.Dtos.Subject;
using TutoringSystem.Application.Models.Dtos.Token;
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
            services.AddSettings(configuration);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
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
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(settings);
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
                    ValidIssuer = settings.JwtIssuer,
                    ValidAudience = settings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey))
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
            services.AddSingleton<IHostedService, InactivedAccountsDeletion>();

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

            services.AddScoped<IValidator<NewStudentDto>, NewStudentValidator>();
            services.AddScoped<IValidator<RegisteredTutorDto>, RegisteredTutorValidator>();
            services.AddScoped<IValidator<NewSubjectDto>, SubjectCreationValidator>();
            services.AddScoped<IValidator<UpdatedSubjectDto>, SubjectEditionValidator>();
            services.AddScoped<IValidator<RegisteredStudentDto>, RegisteredStudentValidator>();
            services.AddScoped<IValidator<PasswordDto>, PasswordChangeValidator>();
            services.AddScoped<IValidator<TokenRefreshRequestDto>, RefreshTokenValidator>();
            services.AddScoped<IValidator<NewPasswordDto>, PasswordResetValidator>();
            services.AddScoped<IValidator<ActivationTokenDto>, ActivationTokenValidator>();

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

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            return services;
        }
    }
}