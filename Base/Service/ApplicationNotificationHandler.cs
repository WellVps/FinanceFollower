using System.Diagnostics.CodeAnalysis;
using BaseService.Interfaces;
using BaseService.Models;
using FluentValidation.Results;

namespace BaseService;

public class ApplicationNotificationHandler : IApplicationNotificationHandler
{
    private readonly List<ApplicationNotification> _notifications = [];

    public void HandleError(string message) => AddNotification(message, isError: true);

    public void HandleError(string message, params object?[] formatValues) => HandleError(string.Format(message, formatValues));

    public void HandleErrors(IEnumerable<string> messages)
    {
        foreach (var message in messages) HandleError(message);
    }

    public void HandleErrors(ValidationResult validationResult)
    {
        if(validationResult.IsValid) return;

        var errors = validationResult.Errors.Select(x => x.ErrorMessage);
        HandleErrors(errors);
    }

    public void HandleSuccess(string message) => AddNotification(message);

    public void HandleSuccess(string message, params object?[] formatValues) => HandleSuccess(string.Format(message, formatValues));

    private void AddNotification(string message, bool isError = false)
    {
        if(_notifications.TrueForAll(m => m.Message != message))
        {
            _notifications.Add(new ApplicationNotification(message, isError));
        }
    }

    public bool HasErrors() => _notifications.Exists(n => n.IsError);
    public bool HasSuccess() => _notifications.Count != 0 && _notifications.TrueForAll(n => !n.IsError);
    
    public List<ApplicationNotification> GetNotifications() => _notifications;

    public void Clean() => _notifications.Clear();
}