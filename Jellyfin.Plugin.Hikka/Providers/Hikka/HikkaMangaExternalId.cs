using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaMangaExternalId : IExternalId
{
  public bool Supports(IHasProviderIds item)
      => item is Book;

  public string ProviderName
      => ProviderNames.HikkaManga;

  public string Key
      => ProviderNames.HikkaManga;

  public ExternalIdMediaType? Type
      => ExternalIdMediaType.Series;

  public string UrlFormatString
      => "https://hikka.io/manga/{0}";
}
