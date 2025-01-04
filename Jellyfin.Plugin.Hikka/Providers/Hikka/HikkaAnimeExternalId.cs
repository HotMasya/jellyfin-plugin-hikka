using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaAnimeExternalId : IExternalId
{
  public bool Supports(IHasProviderIds item)
      => item is Series || item is Movie;

  public string ProviderName
      => ProviderNames.HikkaAnime;

  public string Key
      => ProviderNames.HikkaAnime;

  public ExternalIdMediaType? Type
      => ExternalIdMediaType.Series;

  public string UrlFormatString
      => "https://hikka.io/anime/{0}";
}
