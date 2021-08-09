using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutoringSystem.Application.DependencyInjection;
using TutoringSystem.Infrastructure.DependencyInjection;

namespace TutoringSystem.API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication(configuration);
            services.AddInfrastructure();
        }
    }
}
