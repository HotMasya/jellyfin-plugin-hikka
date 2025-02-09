using Jellyfin.Plugin.Hikka.Utils;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class MediaWithTitle
{
    public string? TitleUa { get; set; }

    public string? TitleEn { get; set; }

    public string? GetPreferredTitle()
    {
        return LanguageUtils.GetPreferredStringValue(TitleUa, TitleEn);
    }
}
