using BaseApi.Auth.Enums;
using BaseApi.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace BaseApi.Auth.Handlers;

public class AdministratorAuthorizationHandler : BaseAuthenticationHandler<AdministratorRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorRequirement requirement)
    {
        if(VerifyRule(context, AccessRoles.Client))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}