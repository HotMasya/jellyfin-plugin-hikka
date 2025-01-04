namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ContentType
{
    private ContentType(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static ContentType Anime
    {
        get { return new ContentType("anime"); }
    }

    public static ContentType Manga
    {
        get { return new ContentType("manga"); }
    }

    public static ContentType Novel
    {
        get { return new ContentType("novel"); }
    }

    public static ContentType Character
    {
        get { return new ContentType("character"); }
    }

    public static ContentType Company
    {
        get { return new ContentType("company"); }
    }

    public static ContentType Episode
    {
        get { return new ContentType("episode"); }
    }

    public static ContentType Genre
    {
        get { return new ContentType("genre"); }
    }

    public static ContentType Person
    {
        get { return new ContentType("person"); }
    }

    public static ContentType Staff
    {
        get { return new ContentType("staff"); }
    }

    public static ContentType Edit
    {
        get { return new ContentType("edit"); }
    }

    public static ContentType Collection
    {
        get { return new ContentType("collection"); }
    }

    public static ContentType Comment
    {
        get { return new ContentType("comment"); }
    }

    public static ContentType Article
    {
        get { return new ContentType("article"); }
    }

    public override string ToString()
    {
        return Value;
    }
}
