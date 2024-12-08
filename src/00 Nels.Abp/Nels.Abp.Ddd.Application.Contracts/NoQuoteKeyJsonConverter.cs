using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nels.Abp.Ddd.Application.Contracts;

public class NoQuoteKeyJsonConverter : JsonConverter<Dictionary<string, object>>
{
    public override Dictionary<string, object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var dictionary = new Dictionary<string, object>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return dictionary;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            reader.Read();
            dictionary[propertyName] = JsonSerializer.Deserialize<object>(ref reader, options);
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var key in value.Keys)
        {
            writer.WritePropertyName(key);
            JsonSerializer.Serialize(writer, value[key], options);
        }

        writer.WriteEndObject();
    }
}
