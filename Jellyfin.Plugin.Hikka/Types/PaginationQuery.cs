namespace Jellyfin.Plugin.Hikka.Types;

public class PaginationQuery
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = 24;
}
