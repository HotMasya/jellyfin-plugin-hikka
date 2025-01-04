using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ReleaseStatusesJsonConverter : JsonArrayConverter<ReleaseStatus>
{
  public ReleaseStatusesJsonConverter() : base(new ReleaseStatusJsonConverter())
  {
  }
}
