using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace BaseDomain;

public class NotificationDomain
{
    public List<string> Messages { get;} = [];
    public bool IsValid => Messages.Count == 0;

    public void AddNotification(string message)
    {
        if (!Messages.Contains(message))
            Messages.Add(message);
    }

    public void AddNotification([StringSyntax("CompositeFormat")] string message, params object?[] formatValues) =>
        AddNotification(string.Format(message, formatValues));

    public void AddValidationErrors(List<ValidationFailure> failures)
    {
        foreach (var failure in failures)
            AddNotification($"{failure.PropertyName}: {failure.ErrorMessage}");
    }
}