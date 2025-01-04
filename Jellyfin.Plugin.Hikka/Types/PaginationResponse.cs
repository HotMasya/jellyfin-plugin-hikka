namespace Jellyfin.Plugin.Hikka.Types;

public class PaginationResponse<T>
{
  public List<T> List { get; set; }
  public Pagination Pagination { get; set; }
}
