using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types;

public class NovelSearchResult : SearchResultBase
{
  public string? TitleOriginal { get; set; }

  public int? Chapters { get; set; }

  public int? Volumes { get; set; }
}
