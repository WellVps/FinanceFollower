namespace JobsService.Base;

public interface IJobManager
{
    void RegisterBackgroundJobs();
    void RegisterManualJobs();
}
