using Jellyfin.Plugin.Hikka.Utils;

namespace Jellyfin.Plugin.Hikka.Types;

public class StaffMemberRole
{
    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public int? Weight { get; set; }

    public required string Slug { get; set; }

    public string? GetPreferredName()
    {
        return LanguageUtils.GetPreferredStringValue(NameUa, NameEn);
    }
}
