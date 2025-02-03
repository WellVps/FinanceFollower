using Microsoft.AspNetCore.Authorization;

namespace BaseApi.Auth.Requirements;

public record struct AdministratorRequirement : IAuthorizationRequirement;