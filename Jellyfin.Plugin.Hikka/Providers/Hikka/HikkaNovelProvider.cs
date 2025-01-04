using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaNovelProvider : IRemoteMetadataProvider<Book, BookInfo>, IHasOrder
{
  private readonly ILogger _log;
  private readonly HikkaApi _hikkaApi;

  public string Name => ProviderNames.HikkaNovel;
  public int Order => -2;

  public HikkaNovelProvider(ILogger<HikkaNovelProvider> logger)
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
    Novel novel = null;
    var mediaId = info.ProviderIds.GetOrDefault(Name);

    if (!string.IsNullOrEmpty(mediaId))
    {
      _log.LogInformation("Media id \"{mediaId}\" found. Loading metadata.", mediaId);
      novel = await _hikkaApi.GetNovelAsync(mediaId, cancellationToken);
    }
    else
    {
      var searchName = SearchHelpers.PreprocessTitle(info.Name);

      _log.LogInformation("Searching for {searchName}...", searchName);
      var searchResults = await _hikkaApi.SearchNovelAsync(new NovelSearchArgs
      {
        Query = searchName,
      }, cancellationToken);

      if (searchResults.Pagination.Total > 0)
      {
        var searchResult = searchResults.List[0];
        _log.LogInformation("Found novel metadata for \"{resultName}\"", searchResult.TitleUa);
        novel = await _hikkaApi.GetNovelAsync(searchResult.Slug, cancellationToken);
      }
    }

    if (novel != null)
    {
      result.HasMetadata = true;
      result.Item = novel.ToBook(Name);
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
      var novel = await _hikkaApi.GetNovelAsync(mediaId, cancellationToken);

      if (novel != null) {
        results.Add(novel.ToSearchResult(Name));
      }
    }

    if (!string.IsNullOrEmpty(searchInfo.Name))
    {
      _log.LogInformation("Searching for {searchName}...", searchInfo.Name);
      var searchResults = await _hikkaApi.SearchNovelAsync(new NovelSearchArgs
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
