using System;
using System.Linq;
using System.Reflection;
using TuyaSharp.Attributes;

namespace TuyaSharp.Utils;

public static class CommandValueAttributeHelper
{
    public static string GetCommandValue(Enum obj)
    {
        var type = obj.GetType();
        var memberInfo = type.GetMember(obj.ToString());
        var attributes = memberInfo[0].GetCustomAttributes<CommandValueAttribute>();
        return attributes.FirstOrDefault()?.Value
               ?? throw new NullReferenceException(
                   $"Enum {type.Name} has a CommandValueAttribute but the value is null");
    }
}
