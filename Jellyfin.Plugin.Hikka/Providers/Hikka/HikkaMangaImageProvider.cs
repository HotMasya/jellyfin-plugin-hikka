using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaMangaImageProvider : HikkaImageProvider
{
    public HikkaMangaImageProvider(ILogger<HikkaImageProvider> logger) : base(logger)
    {
      Name = ProviderNames.HikkaManga;
    }
}
