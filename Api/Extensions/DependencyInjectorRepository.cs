using BaseInfraestructure.Persistence.Helpers;
using Infraestructure.MongoContexts.Interfaces;

namespace Api.Extensions;

public static class DependencyInjectorRepository
{
    public static void AddRepositories(this IServiceCollection services)
    {
        DependencyInjectionHelper.RegistesRepositories(typeof(IMainContext), services);
    }
}