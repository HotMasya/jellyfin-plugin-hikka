using Jellyfin.Plugin.Hikka.Types.Abstract;

using JellyfinEpisode = MediaBrowser.Controller.Entities.TV.Episode;

namespace Jellyfin.Plugin.Hikka.Types;

public class Episode : MediaWithTitle
{
    public long? Aired { get; set; }

    public string? TitleJa { get; set; }

    public int Index { get; set; }

    private DateTime? GetAiredDate()
    {
        if (Aired.HasValue)
        {
            return MediaBase.UnixTimeToDateTime(Aired.Value);
        }

        return null;
    }

    public JellyfinEpisode ToEpisode()
    {
        return new JellyfinEpisode
        {
            Name = GetPreferredTitle(),
            OriginalTitle = TitleJa,
            PremiereDate = GetAiredDate(),
            IndexNumber = Index,
        };
    }
}
