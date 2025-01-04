using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types;

public class Genre
{
    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public required string Slug { get; set; }

    [JsonConverter(typeof(ContentTypeJsonConverter))]
    public required ContentType Type { get; set; }
}
