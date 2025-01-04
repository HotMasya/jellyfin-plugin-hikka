using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class MediaTypeJsonConverter : JsonConverter<MediaType>
{
    public override MediaType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return value switch
        {
            "special" => MediaType.Special,
            "movie" => MediaType.Movie,
            "music" => MediaType.Music,
            "ova" => MediaType.Ova,
            "ona" => MediaType.Ona,
            "tv" => MediaType.Tv,

            "light_novel" => MediaType.LightNovel,
            "novel" => MediaType.Novel,

            "one_shot" => MediaType.OneShot,
            "doujin" => MediaType.Doujin,
            "manhua" => MediaType.Manhua,
            "manhwa" => MediaType.Manhwa,
            "manga" => MediaType.Manga,

            _ => throw new JsonException($"Unknown MediaType value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, MediaType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
