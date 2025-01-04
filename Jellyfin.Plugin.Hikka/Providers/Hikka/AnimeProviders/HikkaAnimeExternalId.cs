using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.AnimeProviders;

public class HikkaAnimeExternalId : IExternalId
{
    public string ProviderName
        => ProviderNames.HikkaAnime;

    public string Key
        => ProviderNames.HikkaAnime;

    public ExternalIdMediaType? Type
        => ExternalIdMediaType.Series;

    public string UrlFormatString
        => "https://hikka.io/anime/{0}";

    public bool Supports(IHasProviderIds item)
        => item is Series || item is Movie;
}
