using BaseApi.Auth.Enums;
using BaseApi.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace BaseApi.Auth.Handlers;

public class ClientAuthorizationHandler : BaseAuthenticationHandler<ClientRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientRequirement requirement)
    {
        if(VerifyRule(context, AccessRoles.Client))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}