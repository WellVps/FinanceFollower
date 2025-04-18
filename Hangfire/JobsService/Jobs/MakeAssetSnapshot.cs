using Hangfire;
using JobsService.Base;
using JobsService.Consts;
using JobsService.Services.Interfaces;

namespace JobsService.Jobs;

public class UpdateAssetsWithSnapshot: IBackgroundJob
{
    public void ScheduleJob()
    {
        RecurringJob.AddOrUpdate<IUpdateAssetsService>(
            JobsIdentifier.JobUpdateAssetsWithSnapshot,
            x => x.UpdateAssets(true),
            "0 21 * * *" // Every day at 21:00 UTC
        );
    }
}
