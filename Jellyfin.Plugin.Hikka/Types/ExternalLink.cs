namespace Jellyfin.Plugin.Hikka.Types;

public class ExternalLink
{
    public required string Url { get; set; }

    public required string Text { get; set; }

    public required string Type { get; set; }
}
