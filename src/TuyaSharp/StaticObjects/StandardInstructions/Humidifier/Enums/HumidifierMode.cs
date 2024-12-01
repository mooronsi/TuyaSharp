using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierMode
{
    [CommandValue("large")]
    Large,

    [CommandValue("middle")]
    Middle,

    [CommandValue("small")]
    Small,

    [CommandValue("interval")]
    Interval,

    [CommandValue("continuous")]
    Continuous
}
