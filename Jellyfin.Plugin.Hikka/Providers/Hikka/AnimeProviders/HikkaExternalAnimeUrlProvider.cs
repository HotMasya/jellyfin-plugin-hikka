using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.AnimeProviders;

/// <summary>
/// External url provider for Hikka.
/// </summary>
public class HikkaExternalAnimeUrlProvider : IExternalUrlProvider
{
    public string Name => ProviderNames.HikkaAnime;

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item.TryGetProviderId(ProviderNames.HikkaAnime, out var externalId))
        {
            switch (item)
            {
                case Series:
                case Movie:
                    yield return $"https://hikka.io/anime/{externalId}";
                    break;
            }
        }
    }
}
