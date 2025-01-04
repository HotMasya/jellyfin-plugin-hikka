using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ContentType
{
    private ContentType(string value) { Value = value; }

    public string Value { get; private set; }

    public static ContentType Anime { get { return new ContentType("anime"); } }
    public static ContentType Manga { get { return new ContentType("manga"); } }
    public static ContentType Novel { get { return new ContentType("novel"); } }
    public static ContentType Character { get { return new ContentType("character"); } }
    public static ContentType Company { get { return new ContentType("company"); } }
    public static ContentType Episode { get { return new ContentType("episode"); } }
    public static ContentType Genre { get { return new ContentType("genre"); } }
    public static ContentType Person { get { return new ContentType("person"); } }
    public static ContentType Staff { get { return new ContentType("staff"); } }
    public static ContentType Edit { get { return new ContentType("edit"); } }
    public static ContentType Collection { get { return new ContentType("collection"); } }
    public static ContentType Comment { get { return new ContentType("comment"); } }
    public static ContentType Article { get { return new ContentType("article"); } }

    public override string ToString()
    {
        return Value;
    }
}

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

public class ContentTypesJsonConverter : JsonArrayConverter<ContentType>
{
    public ContentTypesJsonConverter() : base(new ContentTypeJsonConverter()) { }
}
