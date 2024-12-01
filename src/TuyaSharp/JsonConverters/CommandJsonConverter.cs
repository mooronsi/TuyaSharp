using System;
using Newtonsoft.Json;
using TuyaSharp.DTO.Device.DeviceCommand;

namespace TuyaSharp.JsonConverters;

public class CommandJsonConverter : JsonConverter
{
    override public void WriteJson(
        JsonWriter writer,
        object? value,
        JsonSerializer serializer)
    {
        if (value is Command command)
        {
            if (string.IsNullOrEmpty(command.ValueType))
            {
                throw new JsonSerializationException(
                    $"The {nameof(Command)} object must have a {nameof(Command.ValueType)}.");
            }

            writer.WriteStartObject();

            writer.WritePropertyName("code");
            writer.WriteValue(command.Code);

            writer.WritePropertyName("value");

            switch (command.ValueType)
            {
                case "Boolean":
                    writer.WriteValue(bool.Parse(command.Value));
                    break;
                case "Enum" or "String":
                    writer.WriteValue(command.Value);
                    break;
                case "Integer":
                    writer.WriteValue(int.Parse(command.Value));
                    break;
                default:
                    throw new JsonSerializationException(
                        $"The {nameof(Command)} object has an invalid {nameof(Command.ValueType)} - {command.ValueType}.");
            }

            writer.WriteEndObject();
            return;
        }

        throw new ArgumentException("Invalid object type.");
    }

    override public object? ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer) =>
        JsonConvert.DeserializeObject<Command>(reader.Value?.ToString() ?? string.Empty);

    override public bool CanConvert(Type objectType) =>
        objectType == typeof(Command);
}
