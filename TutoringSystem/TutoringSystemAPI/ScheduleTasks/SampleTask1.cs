using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TutoringSystem.API.BackgroundService;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.ScheduleTasks
{
    public class SampleTask1 : ScheduledProcessor
    {
        public SampleTask1(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "*/1 * * * *"; // every 1 min 

        public override async Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            ISubjectService subjectService = scopeServiceProvider.GetRequiredService<ISubjectService>();
            await subjectService.AddSubjectAsync(1, new Application.Dtos.SubjectDtos.NewSubjectDto
            {
                Category = Domain.Entities.Enums.SubjectCategory.Informatics,
                Description = "Desc1",
                Name = "Sub name",
                Place = Domain.Entities.Enums.SubjectPlace.Online
            });

            await Task.Run(() => {
                return Task.CompletedTask;
            });
        }
    }
}
