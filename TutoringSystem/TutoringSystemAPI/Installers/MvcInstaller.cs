using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutoringSystem.Application.DependencyInjection;
using TutoringSystem.Infrastructure.DependencyInjection;

namespace TutoringSystem.API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();

            services.AddControllers();
        }
    }
}
