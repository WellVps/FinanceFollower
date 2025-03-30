using Hangfire.Dashboard;

namespace JobsAPI.Security;

public class HangFireAuthorization : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
