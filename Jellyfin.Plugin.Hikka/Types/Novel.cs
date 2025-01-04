using Jellyfin.Plugin.Hikka.Types.Abstract;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Types;

public class Novel : MediaBase, IBookConvertable
{
    public required IEnumerable<Author> Authors { get; set; }

    public required IEnumerable<Magazine> Magazines { get; set; }

    public required IEnumerable<ExternalLink> External { get; set; }

    public string? TitleOriginal { get; set; }

    // public Stats Stats { get; set; }

    public int? Chapters { get; set; }

    public required IEnumerable<string> Synonyms { get; set; }

    public int CommentsCount { get; set; }

    public int? Volumes { get; set; }

    public Book ToBook(string providerName)
    {
        return new Book
        {
            Name = GetPreferredTitle(),
            OriginalTitle = TitleOriginal,
            Overview = GetPreferredSynopsis(),
            ProductionYear = Year,
            PremiereDate = GetDate(StartDate),
            EndDate = GetDate(EndDate),
            CommunityRating = Score,
            Genres = [.. GetGenreNames()],
            // TODO: Do something with tags
            Tags = [],
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }
}
