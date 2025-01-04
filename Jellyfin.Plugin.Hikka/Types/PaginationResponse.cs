namespace Jellyfin.Plugin.Hikka.Types;

public class PaginationResponse<T>
{
  public required IEnumerable<T> List { get; set; }

  public required Pagination Pagination { get; set; }
}
