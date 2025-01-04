namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ReleaseStatus
{
    private ReleaseStatus(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static ReleaseStatus Discontinued
    {
        get { return new ReleaseStatus("discontinued"); }
    }

    public static ReleaseStatus Announced
    {
        get { return new ReleaseStatus("announced"); }
    }

    public static ReleaseStatus Finished
    {
        get { return new ReleaseStatus("finished"); }
    }

    public static ReleaseStatus Ongoing
    {
        get { return new ReleaseStatus("ongoing"); }
    }

    public static ReleaseStatus Paused
    {
        get { return new ReleaseStatus("paused"); }
    }

    public override string ToString()
    {
        return Value;
    }
}
