using ConfigurationDBCommon.MongoContexts;
using BaseInfraestructure.Persistence.Helpers;
using ConfigurationDBCommon.MongoContexts.Interfaces;

namespace JobsAPI.Extensions;

public static class DependencyInjectorContext
{
    public static void AddMongoContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfigurationContext>(sp => new ConfigurationContext(
            ConfigurationHelper.BuildMongoDatabase("ConfigurationContext", configuration),
            ConfigurationHelper.BuildMongoDatabase("ConfigurationContext", configuration, false)
        ));
    }
}
