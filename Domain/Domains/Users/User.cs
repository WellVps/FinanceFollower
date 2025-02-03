using System.Text.Json.Serialization;
using BaseApi.Auth.Enums;
using BaseDomain.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Domains.Users;

public class User: AuditedBaseEntity
{
    public string Name { get; set;}
    public string Email { get; set;}
    public string Password { get; set;}
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public AccessRoles AuthorizationRole { get; set;}
    public bool Active { get; set;}

    public User(
        string name,
        string email,
        string password,
        AccessRoles authorizationRole,
        bool active = true
    )
    {
        Name = name;
        Email = email;
        Password = password;
        AuthorizationRole = authorizationRole;
        Active = active;
        CreatedAt = DateTime.Now;
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(Name))
        {
            DomainValidation.AddNotification("Name", "Name is required");
        }
    }
}