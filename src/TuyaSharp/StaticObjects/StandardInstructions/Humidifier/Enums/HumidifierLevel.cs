using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierLevel
{
    [CommandValue("level_1")]
    Level1,

    [CommandValue("level_2")]
    Level2,

    [CommandValue("level_3")]
    Level3,

    [CommandValue("level_4")]
    Level4,

    [CommandValue("level_5")]
    Level5,

    [CommandValue("level_6")]
    Level6,

    [CommandValue("level_7")]
    Level7,

    [CommandValue("level_8")]
    Level8,

    [CommandValue("level_9")]
    Level9,

    [CommandValue("level_10")]
    Level10
}
