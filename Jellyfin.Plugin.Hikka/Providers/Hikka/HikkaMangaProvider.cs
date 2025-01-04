using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaMangaProvider : IRemoteMetadataProvider<Book, BookInfo>, IHasOrder
{
  private readonly ILogger _log;
  private readonly HikkaApi _hikkaApi;

  public string Name => ProviderNames.HikkaManga;
  public int Order => -2;

  public HikkaMangaProvider(ILogger<HikkaMangaProvider> logger)
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

  public async Task<MetadataResult<Book>> GetMetadata(BookInfo info, CancellationToken cancellationToken)
  {
    var result = new MetadataResult<Book>();
    Manga manga = null;
    var mediaId = info.ProviderIds.GetOrDefault(Name);

    if (!string.IsNullOrEmpty(mediaId))
    {
      _log.LogInformation("Media id \"{mediaId}\" found. Loading metadata.", mediaId);
      manga = await _hikkaApi.GetMangaAsync(mediaId, cancellationToken);
    }
    else
    {
      var searchName = SearchHelpers.PreprocessTitle(info.Name);

      _log.LogInformation("Searching for {searchName}...", searchName);
      var searchResults = await _hikkaApi.SearchMangaAsync(new MangaSearchArgs
      {
        Query = searchName,
      }, cancellationToken);

      if (searchResults.Pagination.Total > 0)
      {
        var searchResult = searchResults.List[0];
        _log.LogInformation("Found manga metadata for \"{resultName}\"", searchResult.TitleUa);
        manga = await _hikkaApi.GetMangaAsync(searchResult.Slug, cancellationToken);
      }
    }

    if (manga != null)
    {
      result.HasMetadata = true;
      result.Item = manga.ToBook(Name);
      result.Provider = Name;
    }

    return result;
  }

  public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(BookInfo searchInfo, CancellationToken cancellationToken)
  {
    var results = new List<RemoteSearchResult>();

    var mediaId = searchInfo.ProviderIds.GetOrDefault(Name);

    if (!string.IsNullOrEmpty(mediaId))
    {
      _log.LogInformation("Media id \"{mediaId}\" found. Loading metadata.", mediaId);
      var manga = await _hikkaApi.GetMangaAsync(mediaId, cancellationToken);

      if (manga != null) {
        results.Add(manga.ToSearchResult(Name));
      }
    }

    if (!string.IsNullOrEmpty(searchInfo.Name))
    {
      _log.LogInformation("Searching for {searchName}...", searchInfo.Name);
      var searchResults = await _hikkaApi.SearchMangaAsync(new MangaSearchArgs
      {
        Query = searchInfo.Name,
      }, cancellationToken);
      _log.LogInformation("Found {count} results", searchResults.Pagination.Total);

      if (searchResults.Pagination.Total > 0)
      {
        results.AddRange(searchResults.List.Select((searchResult) => searchResult.ToSearchResult(Name)).ToArray());
      }
    }

    return results;
  }
}
