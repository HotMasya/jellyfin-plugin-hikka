using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaNovelImageProvider : HikkaImageProvider
{
    public HikkaNovelImageProvider(ILogger<HikkaNovelImageProvider> logger) : base(logger)
    {
      Name = ProviderNames.HikkaNovel;
    }
}
