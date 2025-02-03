using System.Security.Claims;
using BaseApi.Auth.Enums;
using Microsoft.AspNetCore.Authorization;
using Crosscutting.Extensions;

namespace BaseApi.Auth.Handlers;

public abstract class BaseAuthenticationHandler<TRequirement> : AuthorizationHandler<TRequirement>
    where TRequirement: IAuthorizationRequirement
{
    protected bool VerifyRule(AuthorizationHandlerContext context, AccessRoles rule)
    {
        if (context.User.IsInRole(rule.GetEnumDescription())) return true;

        var roleUser = context.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        var roleEnum = EnumExtensions.GetValueFromDescription<AccessRoles>(roleUser);

        return (int)roleEnum > (int)rule;
    }
}