using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class MediaWithTitle
{
    public string? TitleUa { get; set; }

    public string? TitleEn { get; set; }

    public string? GetPreferredTitle()
    {
        var config = Plugin.Instance!.Configuration;
        string? title;

        switch (config.PreferredLanguage)
        {
            case Language.English:
                title = TitleEn;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = TitleUa;
                }

                break;

            case Language.Ukrainian:
                title = TitleUa;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = TitleEn;
                }

                break;

            default:
                title = TitleUa;
                break;
        }

        return title;
    }
}
