using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TutoringSystem.Application.BackgroundServices
{
    public abstract class ScheduledProcessor : ScopedProcessor
    {
        private CrontabSchedule schedule;
        private DateTime nextRun;

        protected abstract string Schedule { get; }

        public ScheduledProcessor(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            schedule = CrontabSchedule.Parse(Schedule);
            nextRun = schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;

                if (now > nextRun)
                {
                    await Process();

                    nextRun = schedule.GetNextOccurrence(DateTime.Now);
                }

                await Task.Delay(5000, stoppingToken); // 5 seconds delay

            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}