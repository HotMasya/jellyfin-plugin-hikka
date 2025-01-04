namespace Jellyfin.Plugin.Hikka.Types;

public class Author
{
    public required IEnumerable<AuthorRole> Roles { get; set; }

    public required Person Person { get; set; }
}
