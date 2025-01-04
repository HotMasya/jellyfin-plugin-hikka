namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class MediaType
{
    private MediaType(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static MediaType Special
    {
        get { return new MediaType("special"); }
    }

    public static MediaType Movie
    {
        get { return new MediaType("movie"); }
    }

    public static MediaType Music
    {
        get { return new MediaType("music"); }
    }

    public static MediaType Ova
    {
        get { return new MediaType("ova"); }
    }

    public static MediaType Ona
    {
        get { return new MediaType("ona"); }
    }

    public static MediaType Tv
    {
        get { return new MediaType("tv"); }
    }

    public static MediaType LightNovel
    {
        get { return new MediaType("light_novel"); }
    }

    public static MediaType Novel
    {
        get { return new MediaType("novel"); }
    }

    public static MediaType OneShot
    {
        get { return new MediaType("one_shot"); }
    }

    public static MediaType Doujin
    {
        get { return new MediaType("doujin"); }
    }

    public static MediaType Manhua
    {
        get { return new MediaType("manhua"); }
    }

    public static MediaType Manhwa
    {
        get { return new MediaType("manhwa"); }
    }

    public static MediaType Manga
    {
        get { return new MediaType("manga"); }
    }

    public override string ToString()
    {
        return Value;
    }
}
