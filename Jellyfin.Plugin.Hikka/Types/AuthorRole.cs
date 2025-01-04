namespace Jellyfin.Plugin.Hikka.Types;

public class AuthorRole
{
    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public int? Weight { get; set; }

    public required string Slug { get; set; }
}
