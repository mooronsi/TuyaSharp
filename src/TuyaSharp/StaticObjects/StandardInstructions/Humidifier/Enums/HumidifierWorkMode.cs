using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierWorkMode
{
    [CommandValue("white")]
    White,

    [CommandValue("colour")]
    Colour,

    [CommandValue("colourful1")]
    Colourful1
}
