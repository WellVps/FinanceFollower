using System.Security.Claims;
using BaseInfraestructure.AuditModels;
using Microsoft.AspNetCore.Http;

namespace BaseApi.Extensions;

public static class HttpContextExtensions
{
    public static UserIdentification GetUserIdentification(this HttpContext context)
    {
        return new UserIdentification
        {
            Email = context?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Name = context?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Usuário não identificado",
            Authorization = context?.Request?.Headers.Authorization.ToString() ?? string.Empty
        };
    }
}