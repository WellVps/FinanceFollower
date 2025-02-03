using BaseContract;
using BaseInfraestructure.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaseInfraestructure.Persistence.Helpers;

public static class DependencyInjectionHelper
{
    private static List<Type> ServiceRegistrationExcepts =>
    [
        typeof(IRequestHandler<>),
        typeof(IRequest),
        typeof(IEventHandler),
        typeof(IEquatable<>),
        typeof(IId)
    ];

    public static void RegistesRepositories(Type dbContext, IServiceCollection services)
    {
        var repositoriesWithinAssembly = (
            from type in dbContext.Assembly.GetExportedTypes().Where(x => !x.IsInterface)
            where type.GetInterfaces().Length > 0
            where type.GetConstructors().Length > 0
            where type.GetConstructors()[0].GetParameters().Length > 0
            where type.GetConstructors()[0].GetParameters()[0].ParameterType == dbContext
            select new
            {
                Interfaces = type.GetInterfaces().Where(x => !x.IsGenericType).ToList(),
                Implementation = type
            }
        ).ToList();

        foreach(var repository in repositoriesWithinAssembly)
        {
            foreach(var @interface in repository.Interfaces)
            {
                services.AddScoped(@interface, repository.Implementation);
            }
        }
    }

    public static void RegisterServices(Type startingType, IServiceCollection services, List<Type> excepts = default)
    {
        excepts ??= [];
        excepts.AddRange(ServiceRegistrationExcepts);

        var serviccesWithinAssembly = startingType.Assembly.GetExportedTypes()
            .Where(type => !type.IsInterface && type.GetInterfaces().Length != 0)
            .Where(type => 
            {
                foreach(var except in excepts)
                {
                    foreach (var @interface in type.GetInterfaces())
                    {
                        if(@interface.IsGenericType && @interface.GetGenericTypeDefinition() == except)
                            return false;
                        
                        if(except == @interface)
                            return false;
                    }
                }

                return true;
            })
            .Where(type => type.IsClass)
            .Select(type => new {
                Interfaces = type.GetInterfaces(),
                Implementation = type
            })
            .ToList();

        foreach(var registration in serviccesWithinAssembly)
        {
            foreach(var @interface in registration.Interfaces)
            {
                services.AddScoped(@interface, registration.Implementation);
            }
        }
    }
}