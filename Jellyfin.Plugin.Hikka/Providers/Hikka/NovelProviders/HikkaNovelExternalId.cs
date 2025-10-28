using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.NovelProviders;

public class HikkaNovelExternalId : IExternalId
{
    public string ProviderName
        => ProviderNames.HikkaNovel;

    public string Key
        => ProviderNames.HikkaNovel;

    public ExternalIdMediaType? Type
        => ExternalIdMediaType.Book;

    public bool Supports(IHasProviderIds item)
        => item is Book;
}
