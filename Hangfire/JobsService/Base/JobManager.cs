using BaseService;
using BaseService.Interfaces;

namespace JobsService.Base;

public class JobManager : ApplicationService, IJobManager
{
    private readonly IServiceProvider _provider;
    public JobManager(
        IApplicationNotificationHandler notificationHandler,
        IServiceProvider provider) : base(notificationHandler)
    {
        _provider = provider;    
    }

    public void RegisterBackgroundJobs()
    {
        RegisterJobByContract(typeof(IBackgroundJob));
    }

    public void RegisterManualJobs()
    {
        RegisterJobByContract(typeof(IManualJob));
    }

    private void RegisterJobByContract(Type contractType)
    {
        var typeJobs = contractType
            .Assembly
            .GetExportedTypes()
            .Where(x => !x.IsInterface)
            .Where(x => x.GetInterfaces().Contains(contractType))
            .ToList();

        foreach (var typeJob in typeJobs)
        {
            var job = _provider.GetService(typeJob);
            var method = job?.GetType().GetMethod("ScheduleJob");
            method?.Invoke(job, method.GetParameters());
        }
    }
}
