using System.Net.Http.Headers;
using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using JellyfinEpisode = MediaBrowser.Controller.Entities.TV.Episode;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.AnimeProviders;

public class HikkaEpisodeProvider : IRemoteMetadataProvider<JellyfinEpisode, EpisodeInfo>, IHasOrder
{
    private readonly ILogger _log;
    private readonly HikkaApi _hikkaApi;
    private static Dictionary<string, IEnumerable<Episode>> episodesCache = new();

    public HikkaEpisodeProvider(ILogger<HikkaEpisodeProvider> logger, IMemoryCache cache)
    {
        _log = logger;
        _hikkaApi = new HikkaApi();
    }

    public string Name => ProviderNames.HikkaAnime;

    public int Order => -2;

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();
        var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

        if (response.Content.Headers.ContentType == null)
        {
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        }

        return response;
    }

    public async Task<MetadataResult<JellyfinEpisode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
    {
        var result = new MetadataResult<JellyfinEpisode>();
        IEnumerable<Episode>? episodes = null;
        var seriesId = info.SeriesProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(seriesId))
        {
            _log.LogInformation("Series id \"{MediaId}\" found. Loading episodes metadata.", seriesId);
            episodes = episodesCache.GetValueOrDefault(seriesId);

            if (episodes == null)
            {
                episodes = await _hikkaApi.GetAllAnimeEpisodesAsync(seriesId, cancellationToken).ConfigureAwait(false);

                if (episodes != null)
                {
                    episodesCache.Add(seriesId, episodes);
                }
            }
        }

        if (episodes != null)
        {
            var episode = episodes.FirstOrDefault(e => e.Index == info.IndexNumber)?.ToEpisode();

            if (episode != null)
            {
                result.HasMetadata = true;
                result.Item = episode;
                result.Provider = Name;
            }
        }

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken)
    {
        var results = new List<RemoteSearchResult>();

        var mediaId = searchInfo.SeriesProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(mediaId))
        {
            _log.LogInformation("Media id \"{MediaId}\" found. Loading metadata.", mediaId);
            var media = await _hikkaApi.GetAnimeAsync(mediaId, cancellationToken).ConfigureAwait(false);

            if (media != null)
            {
                results.Add(media.ToSearchResult(Name));
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
