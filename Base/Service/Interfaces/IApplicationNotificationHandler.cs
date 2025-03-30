using System.Diagnostics.CodeAnalysis;
using BaseService.Models;
using FluentValidation.Results;

namespace BaseService.Interfaces;

public interface IApplicationNotificationHandler
{
    bool HasErrors();
    bool HasSuccess();
    List<ApplicationNotification> GetNotifications();
    void Clean();
    void HandleError(string message);
    void HandleError([StringSyntax("CompositeFormat")] string message, params object?[] formatValues);
    void HandleErrors(IEnumerable<string> messages);
    void HandleErrors(ValidationResult validationResult);
    void HandleSuccess(string message);
    void HandleSuccess([StringSyntax("CompositeFormat")] string message, params object?[] formatValues);
}
