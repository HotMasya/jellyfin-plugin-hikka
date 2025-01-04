using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Enums;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public abstract class MediaBase
{
    public string? SynopsisEn { get; set; }

    public string? SynopsisUa { get; set; }

    public bool HasFranchise { get; set; }

    public bool TranslatedUa { get; set; }

    public long? StartDate { get; set; }

    public long? EndDate { get; set; }

    public long Updated { get; set; }

    public string? TitleUa { get; set; }

    public string? TitleEn { get; set; }

    public string? Image { get; set; }

    public int? Year { get; set; }

    public int ScoredBy { get; set; }

    public float Score { get; set; }

    public required string Slug { get; set; }

    [JsonConverter(typeof(ReleaseStatusJsonConverter))]
    public ReleaseStatus? Status { get; set; }

    [JsonConverter(typeof(MediaTypeJsonConverter))]
    public MediaType? MediaType { get; set; }

    public required IEnumerable<Genre> Genres { get; set; }

    public int MalId { get; set; }

    public bool Nsfw { get; set; }

    protected static DateTime UnixTimeToDateTime(long unixTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return dateTimeOffset.UtcDateTime;
    }

    protected DateTime? GetDate(long? unixTime)
    {
        if (unixTime.HasValue)
        {
            return UnixTimeToDateTime(unixTime.Value);
        }

        return null;
    }

    protected IEnumerable<string?> GetGenreNames()
    {
        return Genres.Select((genre) => genre.NameUa);
    }

    public RemoteSearchResult ToSearchResult(string providerName)
    {
        return new RemoteSearchResult
        {
            Name = TitleUa,
            ProductionYear = Year,
            ImageUrl = Image,
            SearchProviderName = providerName,
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }

    public string? GetPreferredTitle()
    {
        var config = Plugin.Instance!.Configuration;
        string? title;

        switch (config.PreferredLanguage)
        {
            case Language.English:
                title = TitleEn;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = TitleUa;
                }

                break;

            case Language.Ukrainian:
                title = TitleUa;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = TitleEn;
                }

                break;

            default:
                title = TitleUa;
                break;
        }

        return title;
    }

    public string? GetPreferredSynopsis()
    {
        var config = Plugin.Instance!.Configuration;
        string? title;

        switch (config.PreferredLanguage)
        {
            case Language.English:
                title = SynopsisEn;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = SynopsisUa;
                }

                break;

            case Language.Ukrainian:
                title = SynopsisUa;

                if (!config.ForcePreferredLanguage && string.IsNullOrEmpty(title))
                {
                    title = SynopsisEn;
                }

                break;

            default:
                title = SynopsisUa;
                break;
        }

        return title;
    }
}
