using System;

namespace TuyaSharp.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CommandValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}
