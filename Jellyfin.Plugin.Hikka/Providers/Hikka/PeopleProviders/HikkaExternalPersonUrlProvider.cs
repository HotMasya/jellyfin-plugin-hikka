using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.PeopleProviders;

/// <summary>
/// External url provider for Hikka.
/// </summary>
public class HikkaExternalPersonUrlProvider : IExternalUrlProvider
{
    public string Name => ProviderNames.HikkaPeople;

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item.TryGetProviderId(ProviderNames.HikkaPeople, out var externalId))
        {
            switch (item)
            {
                case Person:
                    yield return $"https://hikka.io/people/{externalId}";
                    break;
            }
        }
    }
}
