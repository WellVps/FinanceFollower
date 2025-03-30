using System;
using Hangfire;
using JobsService.Base;
using JobsService.Consts;
using JobsService.Services.Interfaces;

namespace JobsService.Jobs;

public class Teste : IBackgroundJob
{
    public void ScheduleJob()
    {
        RecurringJob.AddOrUpdate<ITesteService>(
            JobsIdentifier.JobTeste,
            x => x.ExecTeste(),
            Cron.MinuteInterval(5)
        );
    }
}
