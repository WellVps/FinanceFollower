using BaseInfraestructure.Persistence.Helpers;
using Service.Cqrs.Commands.Users.Handlers;
using Service.Services.Authentication;
using Service.Services.Authentication.Interfaces;
using Service.Services.Users;

namespace Api.Extensions;

public static class DependencyInjectorServices
{
    public static void AddServices(this IServiceCollection services)
    {
        DependencyInjectionHelper.RegisterServices(
            startingType: typeof(SaveUserService),
            services: services
        );

        RegisterSpecific(services);
    }

    private static void RegisterSpecific(IServiceCollection services)
    {
        services.AddMediatR(cfg => {cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly);});

        services.AddSingleton<IPasswordHasher, Md5PasswordHasher>();
    }
}