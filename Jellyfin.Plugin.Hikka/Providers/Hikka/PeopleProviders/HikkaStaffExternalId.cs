using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.PeopleProviders;

public class HikkaStaffExternalId : IExternalId
{
    public string ProviderName
        => ProviderNames.HikkaPeople;

    public string Key
        => ProviderNames.HikkaPeople;

    public ExternalIdMediaType? Type
        => ExternalIdMediaType.Person;

    public bool Supports(IHasProviderIds item)
        => item is Person;
}
