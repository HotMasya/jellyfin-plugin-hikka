namespace Jellyfin.Plugin.Hikka.Types;

public class Genre
{
    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public required string Slug { get; set; }
}
