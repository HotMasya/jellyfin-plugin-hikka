using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.MangaProviders;

/// <summary>
/// External url provider for Hikka.
/// </summary>
public class HikkaExternalMangaUrlProvider : IExternalUrlProvider
{
    public string Name => ProviderNames.HikkaManga;

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item.TryGetProviderId(ProviderNames.HikkaManga, out var externalId))
        {
            switch (item)
            {
                case Book:
                    yield return $"https://hikka.io/manga/{externalId}";
                    break;
            }
        }
    }
}
