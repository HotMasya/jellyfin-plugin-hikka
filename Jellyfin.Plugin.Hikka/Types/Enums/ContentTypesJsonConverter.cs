using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ContentTypesJsonConverter : JsonArrayConverter<ContentType>
{
    public ContentTypesJsonConverter() : base(new ContentTypeJsonConverter())
    {
    }
}
