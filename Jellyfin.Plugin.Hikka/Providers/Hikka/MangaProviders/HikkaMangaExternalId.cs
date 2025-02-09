using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.MangaProviders;

public class HikkaMangaExternalId : IExternalId
{
    public string ProviderName
        => ProviderNames.HikkaManga;

    public string Key
        => ProviderNames.HikkaManga;

    public ExternalIdMediaType? Type
        => ExternalIdMediaType.Book;

    public string UrlFormatString
        => "https://hikka.io/manga/{0}";

    public bool Supports(IHasProviderIds item)
        => item is Book;
}
