using Jellyfin.Plugin.Hikka.Types.Enums;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Hikka.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        PreferredLanguage = Language.Ukrainian;
        ForcePreferredLanguage = false;
    }

    public Language PreferredLanguage { get; set; }

    public bool ForcePreferredLanguage { get; set; }
}
