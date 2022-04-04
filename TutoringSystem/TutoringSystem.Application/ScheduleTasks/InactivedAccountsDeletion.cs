using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.BackgroundServices;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.ScheduleTasks
{
    public class InactivedAccountsDeletion : ScheduledProcessor
    {
        public InactivedAccountsDeletion(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "05 23 * * *";

        public override async Task ProcessInScopeAsync(IServiceProvider scopeServiceProvider)
        {
            var tutorRepository = scopeServiceProvider.GetRequiredService<ITutorRepository>();
            
            var now = DateTime.Now.ToLocal();
            var tutors = await tutorRepository.GetTutorsCollectionAsync(u => !u.IsEnable && u.IsActive && u.RegistrationDate.AddDays(1) < now, isEagerLoadingEnabled: true);
            tutors.ToList().ForEach(u => u.IsActive = false);
            await tutorRepository.UpdateTutorsCollectionAsync(tutors);

            await Task.Run(() =>
            {
                return Task.CompletedTask;
            });
        }
    }
}