using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaNovelExternalId : IExternalId
{
  public bool Supports(IHasProviderIds item)
      => item is Book;

  public string ProviderName
      => ProviderNames.HikkaNovel;

  public string Key
      => ProviderNames.HikkaNovel;

  public ExternalIdMediaType? Type
      => ExternalIdMediaType.Series;

  public string UrlFormatString
      => "https://hikka.io/novel/{0}";
}
