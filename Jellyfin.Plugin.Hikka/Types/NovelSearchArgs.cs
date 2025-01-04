using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types;

public class NovelSearchArgs : SearchArgsBase
{
  public string[] Magazines { get; set; }
}
