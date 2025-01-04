using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types;

public class AnimeSearchResult : SearchResultBase
{
    public string? TitleJa { get; set; }

    public int? EpisodesReleased { get; set; }

    public int? EpisodesTotal { get; set; }

    public string? Season { get; set; }

    public string? Source { get; set; }

    public string? Rating { get; set; }
    // TODO: Implement data type
    // public Array Watch { get; set; }
}
