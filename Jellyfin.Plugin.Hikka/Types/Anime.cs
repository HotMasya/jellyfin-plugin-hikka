using System.Collections.ObjectModel;
using Jellyfin.Plugin.Hikka.Types.Abstract;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;

namespace Jellyfin.Plugin.Hikka.Types;

public class Anime : MediaBase
{
    public IEnumerable<CompanyInfo>? Companies { get; set; }

    public int CommentsCount { get; set; }

    public int? EpisodesReleased { get; set; }

    public int? EpisodesTotal { get; set; }

    public string? TitleJa { get; set; }

    public int? Duration { get; set; }

    public string? Source { get; set; }

    public string? Rating { get; set; }

    public string? Season { get; set; }

    public IEnumerable<string>? Synonyms { get; set; }
    // public List<object> Videos { get; set; }
    // TODO: Define data type
    // public List<object> Ost { get; set; }
    // public Object Stats { get; set; }
    // public List<object> Schedule { get; set; }

    private long? GetDuration()
    {
        if (Duration.HasValue)
        {
            return TimeSpan.FromMinutes(Duration.Value).Ticks;
        }

        return null;
    }

    private IEnumerable<string> GetCompanies()
    {
        if (Companies == null)
        {
            return [];
        }

        return Companies.Select((company) => company.Company.Name);
    }

    public Movie ToMovie(string providerName)
    {
        return new Movie
        {
            Name = TitleUa,
            OriginalTitle = TitleJa,
            Overview = SynopsisUa,
            ProductionYear = Year,
            PremiereDate = GetDate(StartDate),
            EndDate = GetDate(EndDate),
            CommunityRating = Score,
            Genres = [.. GetGenreNames()],
            // TODO: Do something with tags
            Tags = [],
            Studios = [.. GetCompanies()],
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }

    internal Series ToSeries(string providerName)
    {
        return new Series
        {
            Name = TitleUa,
            OriginalTitle = TitleJa,
            Overview = SynopsisUa,
            ProductionYear = Year,
            PremiereDate = GetDate(StartDate),
            EndDate = GetDate(EndDate),
            CommunityRating = Score,
            RunTimeTicks = GetDuration(),
            Genres = [.. GetGenreNames()],
            // TODO: Do something with tags
            Tags = [],
            Studios = [.. GetCompanies()],
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }
}
