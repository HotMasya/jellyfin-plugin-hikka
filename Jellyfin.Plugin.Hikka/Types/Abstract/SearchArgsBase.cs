using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class SearchArgsBase
{
  public int?[] Years { get; set; }
  public string[] MediaType { get; set; }
  [JsonConverter(typeof(ReleaseStatusesJsonCoverter))]
  public List<ReleaseStatus> Status { get; set; }
  public bool OnlyTranslated { get; set; } = false;
  public string[] Genres { get; set; }
  public int?[] Score { get; set; }
  public string Query { get; set; }
  public string[] Sort { get; set; } = ["start_date:asc"];
}
