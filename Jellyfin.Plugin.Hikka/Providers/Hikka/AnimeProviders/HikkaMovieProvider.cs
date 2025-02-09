using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.AnimeProviders;

public class HikkaMovieProvider : IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
{
    private readonly ILogger _log;
    private readonly HikkaApi _hikkaApi;

    public HikkaMovieProvider(ILogger<HikkaMovieProvider> logger)
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

    public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info, CancellationToken cancellationToken)
    {
        var result = new MetadataResult<Movie>();
        Anime? animeMovie = null;
        var animeMovieId = info.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(animeMovieId))
        {
            _log.LogInformation("Anime movie id \"{AnimeMovieId}\" found. Loading metadata.", animeMovieId);
            animeMovie = await _hikkaApi.GetAnimeAsync(animeMovieId, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var searchName = SearchHelpers.PreprocessTitle(Name);

            _log.LogInformation("Searching for {SearchName}...", searchName);
            var searchResults = await _hikkaApi.SearchAnimeAsync(new AnimeSearchArgs { Query = searchName }, cancellationToken).ConfigureAwait(false);

            if (searchResults.Pagination.Total > 0)
            {
                var primaryResult = searchResults.List.First();
                _log.LogInformation("Found anime movie metadata for \"{ResultName}\"", primaryResult.TitleUa);
                animeMovie = await _hikkaApi.GetAnimeAsync(primaryResult.Slug, cancellationToken).ConfigureAwait(false);
            }
        }

        if (animeMovie != null)
        {
            var staffMembers = await _hikkaApi.GetAllAnimeStaffMembers(animeMovie.Slug, cancellationToken).ConfigureAwait(false);

            result.HasMetadata = true;
            result.Item = animeMovie.ToMovie(Name);
            result.People = staffMembers.Select((member) => member.ToPersonInfo(ProviderNames.HikkaPeople)).ToList();
            result.Provider = Name;
        }

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(MovieInfo searchInfo, CancellationToken cancellationToken)
    {
        var results = new List<RemoteSearchResult>();

        var animeMovieId = searchInfo.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(animeMovieId))
        {
            _log.LogInformation("Media id \"{MediaId}\" found. Loading metadata.", animeMovieId);
            var animeMovie = await _hikkaApi.GetAnimeAsync(animeMovieId, cancellationToken).ConfigureAwait(false);

            if (animeMovie != null)
            {
                results.Add(animeMovie.ToSearchResult(Name));
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
