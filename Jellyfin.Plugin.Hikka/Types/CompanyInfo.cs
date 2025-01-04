using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types;

public class CompanyInfo
{
    public required Company Company { get; set; }

    [JsonConverter(typeof(ContentTypeJsonConverter))]
    public required ContentType Type { get; set; }
}
