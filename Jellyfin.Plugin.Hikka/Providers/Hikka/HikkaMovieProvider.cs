using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Entities.Movies;
using System.Net.Http.Headers;

using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaMovieProvider : IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
{
  private readonly ILogger _log;
  private readonly HikkaApi _hikkaApi;

  public string Name => ProviderNames.HikkaAnime;
  public int Order => -2;

  public HikkaMovieProvider(ILogger<HikkaMovieProvider> logger)
  {
    _log = logger;
    _hikkaApi = new HikkaApi();
  }

  public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
  {
    var httpClient = Plugin.Instance.GetHttpClient();
    var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

    if (response.Content.Headers.ContentType == null)
    {
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
    }

    return response;
  }

  public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info, CancellationToken cancellationToken)
  {
    var result = new MetadataResult<Movie>();
    Anime media = null;
    var mediaId = info.ProviderIds.GetOrDefault(Name);

    if (!string.IsNullOrEmpty(mediaId))
    {
      _log.LogInformation("Media id \"{mediaId}\" found. Loading metadata.", mediaId);
      media = await _hikkaApi.GetAnimeAsync(mediaId, cancellationToken);
    }
    else
    {
      var searchName = SearchHelpers.PreprocessTitle(Name);

      _log.LogInformation("Searching for {searchName}...", searchName);
      var searchResults = await _hikkaApi.SearchAnimeAsync(new AnimeSearchArgs
      {
        Query = searchName,
      }, cancellationToken);

      if (searchResults.Pagination.Total > 0)
      {
        var primaryResult = searchResults.List[0];
        _log.LogInformation("Found series metadata for \"{resultName}\"", primaryResult.TitleUa);
        media = await _hikkaApi.GetAnimeAsync(primaryResult.Slug, cancellationToken);
      }
    }

    if (media != null)
    {
      result.HasMetadata = true;
      result.Item = media.ToMovie(Name);
      result.Provider = Name;
    }

    return result;
  }

  public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(MovieInfo searchInfo, CancellationToken cancellationToken)
  {
    var results = new List<RemoteSearchResult>();

    var mediaId = searchInfo.ProviderIds.GetOrDefault(Name);

    if (!string.IsNullOrEmpty(mediaId))
    {
      _log.LogInformation("Media id \"{mediaId}\" found. Loading metadata.", mediaId);
      var media = await _hikkaApi.GetAnimeAsync(mediaId, cancellationToken);

      if (media != null)
      {
        results.Add(media.ToSearchResult(Name));
      }
    }

    if (!string.IsNullOrEmpty(searchInfo.Name))
    {
      _log.LogInformation("Searching for {searchName}...", searchInfo.Name);
      var searchResults = await _hikkaApi.SearchAnimeAsync(new AnimeSearchArgs
      {
        Query = searchInfo.Name,
      }, cancellationToken);
      _log.LogInformation("Found {count} results", searchResults.Pagination.Total);

      if (searchResults.Pagination.Total > 0)
      {
        results.AddRange(searchResults.List.Select((list) => list.ToSearchResult(Name)).ToArray());
      }
    }

    return results;
  }
}
