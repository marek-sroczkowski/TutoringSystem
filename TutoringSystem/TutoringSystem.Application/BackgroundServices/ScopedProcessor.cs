using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TutoringSystem.Application.BackgroundServices
{
    public abstract class ScopedProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory) : base()
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            using var scope = serviceScopeFactory.CreateScope();
            await ProcessInScope(scope.ServiceProvider);
        }

        public abstract Task ProcessInScope(IServiceProvider scopeServiceProvider);
    }
}