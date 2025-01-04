using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types;

public class AnimeSearchArgs: SearchArgsBase
{
  public bool IncludeMultiseason { get; set; } = true;
  public string[] Rating { get; set; }
  public string[] Source { get; set; }
  public string[] Season { get; set; }
  public string[] Producers { get; set; }
  public string[] Studios { get; set; }
}
