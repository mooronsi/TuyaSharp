using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierCountdownSet
{
    [CommandValue("cancel")]
    Cancel,

    [CommandValue("1h")]
    OneHour,

    [CommandValue("2h")]
    TwoHours,

    [CommandValue("3h")]
    ThreeHours,

    [CommandValue("4h")]
    FourHours,

    [CommandValue("5h")]
    FiveHours,

    [CommandValue("6h")]
    SixHours
}
