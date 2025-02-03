using BaseInfraestructure.Persistence.Helpers;
using Infraestructure.MongoContexts;
using Infraestructure.MongoContexts.Interfaces;

namespace Api.Extensions;

public static class DependencyInjectorContext
{
    public static void AddMongoContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMainContext>(sp => new MainContext(
            ConfigurationHelper.BuildMongoDatabase("FinanceFollower", configuration),
            ConfigurationHelper.BuildMongoDatabase("FinanceFollower", configuration, false)
        ));
    }
}