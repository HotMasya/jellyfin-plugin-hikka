using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.NovelProviders;

/// <summary>
/// External url provider for Hikka.
/// </summary>
public class HikkaExternalNovelUrlProvider : IExternalUrlProvider
{
    public string Name => ProviderNames.HikkaNovel;

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item.TryGetProviderId(ProviderNames.HikkaNovel, out var externalId))
        {
            switch (item)
            {
                case Book:
                    yield return $"https://hikka.io/novel/{externalId}";
                    break;
            }
        }
    }
}
