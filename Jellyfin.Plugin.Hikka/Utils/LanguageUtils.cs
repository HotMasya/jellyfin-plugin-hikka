using Jellyfin.Plugin.Hikka.Types.Enums;

namespace Jellyfin.Plugin.Hikka.Utils;

public static class LanguageUtils
{
    public static string? GetPreferredStringValue(string? strUa, string? strEn)
    {
        var config = Plugin.Instance!.Configuration;
        string? title;

        switch (config.PreferredLanguage)
        {
            case Language.English:
                title = strEn;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = strUa;
                }

                break;

            case Language.Ukrainian:
                title = strUa;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = strEn;
                }

                break;

            default:
                title = strUa;
                break;
        }

        return title;
    }
}
