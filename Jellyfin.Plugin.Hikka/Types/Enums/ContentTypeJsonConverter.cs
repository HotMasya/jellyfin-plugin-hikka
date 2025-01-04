using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ContentTypeJsonConverter : JsonConverter<ContentType>
{
    public override ContentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return value switch
        {
            "anime" => ContentType.Anime,
            "manga" => ContentType.Manga,
            "novel" => ContentType.Novel,
            "character" => ContentType.Character,
            "company" => ContentType.Company,
            "episode" => ContentType.Episode,
            "genre" => ContentType.Genre,
            "person" => ContentType.Person,
            "staff" => ContentType.Staff,
            "edit" => ContentType.Edit,
            "collection" => ContentType.Collection,
            "comment" => ContentType.Comment,
            "article" => ContentType.Article,

            _ => throw new JsonException($"Unknown DataType value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ContentType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
