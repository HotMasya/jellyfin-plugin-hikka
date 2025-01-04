using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class MediaTypesJsonConverter : JsonArrayConverter<MediaType>
{
    public MediaTypesJsonConverter() : base(new MediaTypeJsonConverter())
    {
    }
}
