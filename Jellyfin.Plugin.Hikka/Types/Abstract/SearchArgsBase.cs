using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class SearchArgsBase
{
    public IEnumerable<int>? Years { get; set; }

    public IEnumerable<string>? MediaType { get; set; }

    [JsonConverter(typeof(ReleaseStatusesJsonConverter))]
    public IEnumerable<ReleaseStatus>? Status { get; set; }

    public bool OnlyTranslated { get; set; }

    public IEnumerable<string>? Genres { get; set; }

    public IEnumerable<int?>? Score { get; set; }

    public string? Query { get; set; }

    public IEnumerable<string> Sort { get; set; } = ["start_date:asc"];
}
