using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class SearchResultBase
{
  [JsonConverter(typeof(MediaTypeJsonConverter))]
  public MediaType? MediaType { get; set; }

  public string? TitleUa { get; set; }

  public string? TitleEn { get; set; }

  public string? Image { get; set; }

  [JsonConverter(typeof(ReleaseStatusJsonConverter))]
  public ReleaseStatus? Status { get; set; }

  public int ScoredBy { get; set; }

  public float Score { get; set; }

  public required string Slug { get; set; }

  public bool TranslatedUa { get; set; }

  public int? Year { get; set; }

  public RemoteSearchResult ToSearchResult(string providerName)
  {
    return new RemoteSearchResult
    {
      Name = TitleUa,
      ProductionYear = Year,
      ImageUrl = Image,
      SearchProviderName = providerName,
      ProviderIds = new Dictionary<string, string>() { { providerName, Slug } }
    };
  }
}
