using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class JsonArrayConverter<T> : JsonConverter<List<T>>
{
    private readonly JsonConverter<T> _elementConverter;

    public JsonArrayConverter(JsonConverter<T> elementConverter)
    {
        _elementConverter = elementConverter;
    }

    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new List<T>();

        // Ensure that we have a start array token
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        // Read each element in the array
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            // Deserialize each element using the provided element converter
            T element = _elementConverter.Read(ref reader, typeof(T), options);
            list.Add(element);
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (T item in value)
        {
            _elementConverter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
