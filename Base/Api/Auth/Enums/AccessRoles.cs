using System.ComponentModel;

namespace BaseApi.Auth.Enums;

public enum AccessRoles
{
    [Description("Cliente")]
    Client = 1,
    [Description("Administrador")]
    Administrator = 2,
}