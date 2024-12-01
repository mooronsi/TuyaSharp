using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierSprayMode
{
    [CommandValue("auto")]
    Auto,

    [CommandValue("health")]
    Health,

    [CommandValue("baby")]
    Baby,

    [CommandValue("sleep")]
    Sleep,

    [CommandValue("humidity")]
    Humidity,

    [CommandValue("work")]
    Work
}
