using Hangfire;
using JobsService.Base;
using JobsService.Consts;
using JobsService.Services.Interfaces;

namespace JobsService.Jobs;

public class UpdateAssets : IBackgroundJob
{
    public void ScheduleJob()
    {
        RecurringJob.AddOrUpdate<IUpdateAssetsService>(
            JobsIdentifier.JobUpdateAssets,
            x => x.UpdateAssets(false),
            "*/5 8-21 * * *" // Every 5 minutes between 8 AM and 9 PM
        );
    }
}
