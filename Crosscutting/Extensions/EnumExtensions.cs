using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace Crosscutting.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if(field is null) return value.ToString();

        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute is not null ? attribute.Description : value.ToString();
    }

    public static T? GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach(var field in typeof(T).GetFields())
        {
            if(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T?)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T?)field.GetValue(null);
            }
        }

        return default;
    }

    public static string GetEnumMemberValue<T>(this T value) where T : Enum
    {
        return typeof(T)
            .GetTypeInfo()
            .DeclaredMembers
            .SingleOrDefault(x => x.Name == value.ToString())?
            .GetCustomAttribute<EnumMemberAttribute>(false)
            ?.Value ?? string.Empty;
    }

    public static List<string> GetEnumNameList<T>() where T : Enum
        => [.. Enum.GetNames(typeof(T))];

    public static T? GetValueFromEnumValue<T>(string description) where T: Enum
    {
        foreach(var field in typeof(T).GetFields())
        {
            if(Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
            {
                if (attribute.Value == description)
                    return (T?)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T?)field.GetValue(null);
            }
        }

        return default;
    }
}