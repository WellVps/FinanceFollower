using System;
using BaseInfraestructure.Persistence.Helpers;
using BaseService;
using BaseService.Interfaces;
using JobsService.Base;

namespace JobsAPI.Extensions;

public static class DependencyInjectorServices
{
    public static void AddServices(this IServiceCollection services)
    {
        DependencyInjectionHelper.RegisterServices(
            startingType: typeof(JobManager),
            excepts: [],
            services: services
        );
        RegisterSpecific(services);
        RegisterJobs(services, typeof(IManualJob));
        RegisterJobs(services, typeof(IBackgroundJob));
    }

    private static void RegisterSpecific(IServiceCollection services)
    {
        services.AddScoped<IApplicationNotificationHandler, ApplicationNotificationHandler>();
    }

    private static void RegisterJobs(IServiceCollection services, Type typeBase)
    {
        var assemblyBase = typeBase.Assembly;
        var jobsRegistration = 
            from type in assemblyBase.GetExportedTypes().Where(x => !x.IsInterface)
            where type.GetInterfaces().Contains(typeBase)
            select type;

        foreach (var job in jobsRegistration)
            services.AddScoped(job);
    }
}
