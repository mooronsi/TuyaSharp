using TuyaSharp.Attributes;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;

public enum HumidifierTempUnitConvert
{
    [CommandValue("c")]
    Celsius,

    [CommandValue("f")]
    Fahrenheit
}
