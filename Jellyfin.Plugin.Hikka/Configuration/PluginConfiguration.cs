using Jellyfin.Plugin.Hikka.Types.Enums;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Hikka.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public Language PreferredLanguage { get; set; }
}
