using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TutoringSystem.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
