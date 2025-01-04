using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class MediaType
{
  private MediaType(string value) { Value = value; }

  public string Value { get; private set; }

  public static MediaType Special { get { return new MediaType("special"); } }
  public static MediaType Movie { get { return new MediaType("movie"); } }
  public static MediaType Music { get { return new MediaType("music"); } }
  public static MediaType Ova { get { return new MediaType("ova"); } }
  public static MediaType Ona { get { return new MediaType("ona"); } }
  public static MediaType Tv { get { return new MediaType("tv"); } }

  public static MediaType LightNovel { get { return new MediaType("light_novel"); } }
  public static MediaType Novel { get { return new MediaType("novel"); } }

  public static MediaType OneShot { get { return new MediaType("one_shot"); } }
  public static MediaType Doujin { get { return new MediaType("doujin"); } }
  public static MediaType Manhua { get { return new MediaType("manhua"); } }
  public static MediaType Manhwa { get { return new MediaType("manhwa"); } }
  public static MediaType Manga { get { return new MediaType("manga"); } }

  public override string ToString()
  {
    return Value;
  }
}

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

public class MediaTypesJsonCoverter : JsonArrayConverter<MediaType>
{
  public MediaTypesJsonCoverter() : base(new MediaTypeJsonConverter()) { }
}
