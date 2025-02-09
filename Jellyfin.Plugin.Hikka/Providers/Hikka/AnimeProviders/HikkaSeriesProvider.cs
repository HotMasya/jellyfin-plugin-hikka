using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.AnimeProviders;

public class HikkaSeriesProvider : IRemoteMetadataProvider<Series, SeriesInfo>, IHasOrder
{
    private readonly ILogger _log;
    private readonly HikkaApi _hikkaApi;

    public HikkaSeriesProvider(ILogger<HikkaSeriesProvider> logger)
    {
        _log = logger;
        _hikkaApi = new HikkaApi();
    }

    public string Name => ProviderNames.HikkaAnime;

    public int Order => -2;

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken cancellationToken)
    {
        var result = new MetadataResult<Series>();
        Anime? anime = null;
        var animeSeriesId = info.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(animeSeriesId))
        {
            _log.LogInformation("Anime series id \"{AnimeSeriesId}\" found. Loading metadata.", animeSeriesId);
            anime = await _hikkaApi.GetAnimeAsync(animeSeriesId, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var searchName = SearchHelpers.PreprocessTitle(info.Name);

            _log.LogInformation("Searching for {SearchName}...", searchName);
            var searchResults = await _hikkaApi.SearchAnimeAsync(new AnimeSearchArgs { Query = searchName }, cancellationToken).ConfigureAwait(false);

            if (searchResults.Pagination.Total > 0)
            {
                var primaryResult = searchResults.List.First();
                _log.LogInformation("Found anime series metadata for \"{ResultName}\"", primaryResult.TitleUa);
                anime = await _hikkaApi.GetAnimeAsync(primaryResult.Slug, cancellationToken).ConfigureAwait(false);
            }
        }

        if (anime != null)
        {
            var staffMembers = await _hikkaApi.GetAllAnimeStaffMembers(anime.Slug, cancellationToken).ConfigureAwait(false);

            result.HasMetadata = true;
            result.Item = anime.ToSeries(Name);
            result.People = staffMembers.Select((member) => member.ToPersonInfo(ProviderNames.HikkaPeople)).ToList();
            result.Provider = Name;
        }

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo searchInfo, CancellationToken cancellationToken)
    {
        var results = new List<RemoteSearchResult>();

        var animeSeriesId = searchInfo.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(animeSeriesId))
        {
            _log.LogInformation("Anime series id \"{AnimeSeriesId}\" found. Loading metadata.", animeSeriesId);
            var animeSeries = await _hikkaApi.GetAnimeAsync(animeSeriesId, cancellationToken).ConfigureAwait(false);

            if (animeSeries != null)
            {
                results.Add(animeSeries.ToSearchResult(Name));
            }
        }

        if (!string.IsNullOrEmpty(searchInfo.Name))
        {
            _log.LogInformation("Searching for {SearchName}...", searchInfo.Name);
            var searchResults = await _hikkaApi.SearchAnimeAsync(new AnimeSearchArgs { Query = searchInfo.Name }, cancellationToken).ConfigureAwait(false);
            _log.LogInformation("Found {Count} results", searchResults.Pagination.Total);

            if (searchResults.Pagination.Total > 0)
            {
                results.AddRange(searchResults.List.Select((list) => list.ToSearchResult(Name)).ToArray());
            }
        }

        return results;
    }
}
