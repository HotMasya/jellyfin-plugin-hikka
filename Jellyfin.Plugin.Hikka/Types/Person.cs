using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types;

public class Person
{
    [JsonConverter(typeof(ContentTypeJsonConverter))]
    public required ContentType DataType { get; set; }

    public string? NameNative { get; set; }

    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public string? Image { get; set; }

    public required string Slug { get; set; }

    public string? DescriptionUa { get; set; }

    public required IEnumerable<string> Synonyms { get; set; }
}
