using System;
using Newtonsoft.Json;
using TuyaSharp.DTO.Device.DeviceCommand;
using TuyaSharp.StaticObjects.StandardInstructions.Humidifier.Enums;
using TuyaSharp.Utils;

namespace TuyaSharp.StaticObjects.StandardInstructions.Humidifier;

// https://developer.tuya.com/en/docs/iot/f?id=K9gf45rjel6x5
public static class StandardInstructionsHumidifier
{
    public static Command SwitchCommand(bool value) =>
        new()
        {
            Code = "switch",
            Value = value.ToString(),
            ValueType = "Boolean"
        };

    public static Command SwitchSpayCommand(bool value) =>
        new()
        {
            Code = "switch_spray",
            Value = value.ToString(),
            ValueType = "Boolean"
        };

    public static Command ModeCommand(HumidifierMode mode) =>
        new()
        {
            Code = "mode",
            Value = CommandValueAttributeHelper.GetCommandValue(mode),
            ValueType = "Enum"
        };

    public static Command SprayModeCommand(HumidifierSprayMode mode) =>
        new()
        {
            Code = "spray_mode",
            Value = CommandValueAttributeHelper.GetCommandValue(mode),
            ValueType = "Enum"
        };

    public static Command LevelCommand(HumidifierLevel level) =>
        new()
        {
            Code = "level",
            Value = CommandValueAttributeHelper.GetCommandValue(level),
            ValueType = "Enum"
        };

    public static Command CountdownSetCommand(HumidifierCountdownSet countdownSet) =>
        new()
        {
            Code = "countdown_set",
            Value = CommandValueAttributeHelper.GetCommandValue(countdownSet),
            ValueType = "Enum"
        };

    public static Command SwitchLedCommand(bool value) =>
        new()
        {
            Code = "switch_led",
            Value = value.ToString(),
            ValueType = "Boolean"
        };

    public static Command WorkModeCommand(HumidifierWorkMode workMode) =>
        new()
        {
            Code = "work_mode",
            Value = CommandValueAttributeHelper.GetCommandValue(workMode),
            ValueType = "Enum"
        };

    public static Command ColourDataHsvCommand(ushort h, byte s, byte v)
    {
        if (h > 360) throw new ArgumentOutOfRangeException(nameof(h), "Hue must be between 0 and 360");

        return new Command
        {
            Code = "colour_data",
            Value = JsonConvert.SerializeObject(new { h, s, v }),
            ValueType = "Enum"
        };
    }

    public static Command BrightValueCommand(byte value) =>
        new()
        {
            Code = "bright_value",
            Value = value.ToString(),
            ValueType = "Integer"
        };

    public static Command SwitchSoundCommand(bool value) =>
        new()
        {
            Code = "switch_sound",
            Value = value.ToString(),
            ValueType = "Boolean"
        };

    public static Command TempSetCommand(byte value)
    {
        if (value > 50) throw new ArgumentOutOfRangeException(nameof(value), "Temperature must be between 0 and 50");

        return new Command
        {
            Code = "temp_set",
            Value = value.ToString(),
            ValueType = "Integer"
        };
    }

    public static Command TempSetFCommand(byte value)
    {
        if (value > 100) throw new ArgumentOutOfRangeException(nameof(value), "Temperature must be between 0 and 100");

        return new Command
        {
            Code = "temp_set_f",
            Value = value.ToString(),
            ValueType = "Integer"
        };
    }

    public static Command HumiditySetCommand(byte value)
    {
        if (value > 100) throw new ArgumentOutOfRangeException(nameof(value), "Humidity must be between 0 and 100");

        return new Command
        {
            Code = "humidity_set",
            Value = value.ToString(),
            ValueType = "Integer"
        };
    }

    public static Command SleepCommand(bool value) =>
        new()
        {
            Code = "sleep",
            Value = value.ToString(),
            ValueType = "Boolean"
        };

    public static Command TempUnitConvertCommand(HumidifierTempUnitConvert tempUnitConvert) =>
        new()
        {
            Code = "temp_unit_convert",
            Value = CommandValueAttributeHelper.GetCommandValue(tempUnitConvert),
            ValueType = "Enum"
        };

    public static Command SterilizationCommand(bool value) =>
        new()
        {
            Code = "sterilization",
            Value = value.ToString(),
            ValueType = "Boolean"
        };
}
