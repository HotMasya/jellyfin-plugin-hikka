using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types;

public class AnimeSearchArgs : SearchArgsBase
{
    public bool IncludeMultiseason { get; set; } = true;

    public IEnumerable<string>? Rating { get; set; }

    public IEnumerable<string>? Source { get; set; }

    public IEnumerable<string>? Season { get; set; }

    public IEnumerable<string>? Producers { get; set; }

    public IEnumerable<string>? Studios { get; set; }
}
