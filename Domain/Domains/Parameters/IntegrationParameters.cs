using BaseDomain.Entity;

namespace Domain.Domains.Parameters;

public class IntegrationParameters : AuditedBaseEntity
{
    public string Name { get; set; }
    public List<Parameter> Parameters { get; set; }
    public List<Parameter> AuthParameters { get; set; }
    public bool Active { get; set; } = true;

    public IntegrationParameters(string name, List<Parameter> parameters, List<Parameter> authParameters)
    {
        Name = name;
        Parameters = parameters;
        AuthParameters = authParameters;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(Name))
        {
            DomainValidation.AddNotification("Name", "Name is required");
        }

        if (Parameters == null || !Parameters.Any())
        {
            DomainValidation.AddNotification("Parameters", "At least one parameter is required");
        }
        else
        {
            foreach (var parameter in Parameters)
            {
                if (string.IsNullOrEmpty(parameter.Key))
                {
                    DomainValidation.AddNotification("Key", "Key is required");
                }

                if (string.IsNullOrEmpty(parameter.Value))
                {
                    DomainValidation.AddNotification("Value", "Value is required");
                }
            }
        }

        if (AuthParameters == null || !AuthParameters.Any())
        {
            DomainValidation.AddNotification("AuthParameters", "At least one auth parameter is required");
        }
        else
        {
            foreach (var authParameter in AuthParameters)
            {
                if (string.IsNullOrEmpty(authParameter.Key))
                {
                    DomainValidation.AddNotification("Key", "Key is required");
                }

                if (string.IsNullOrEmpty(authParameter.Value))
                {
                    DomainValidation.AddNotification("Value", "Value is required");
                }
            }
        }
    }
}

public class Parameter(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}