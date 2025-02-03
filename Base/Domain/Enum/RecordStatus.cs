using System.ComponentModel;

namespace BaseDomain.Enum;

public enum RecordStatus
{
    [Description("Ativo")]
    Active = 1,
    [Description("Inativo")]
    Inactive = 2
}