namespace Jellyfin.Plugin.Hikka.Types;

public class Company
{
    public string? Image { get; set; }

    public required string Slug { get; set; }

    public required string Name { get; set; }
}
