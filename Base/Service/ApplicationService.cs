using BaseDomain;
using BaseService.Interfaces;

namespace BaseService;

public class ApplicationService
{
    protected readonly IApplicationNotificationHandler _notifications;

    public ApplicationService(IApplicationNotificationHandler notifications)
    {
        _notifications = notifications;
    }

    public void HandleDomainNotifications(NotificationDomain notification)
    {
        if(!notification.IsValid)
            notification.Messages.ForEach(m => _notifications.HandleError(m));
    }
}
