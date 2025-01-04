using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

using Jellyfin.Plugin.Hikka.Utils;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka;

public class HikkaImageProvider : IRemoteImageProvider
{
  public string Name { get; protected set; } = ProviderNames.HikkaAnime;
  private readonly ILogger _log;
  private readonly HikkaApi _hikkaApi;

  public HikkaImageProvider(ILogger<HikkaImageProvider> logger)
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

  public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
  {
    var mediaId = item.ProviderIds.GetOrDefault(Name);
    if (!string.IsNullOrEmpty(mediaId))
    {
      switch (item)
      {
        case Series:
          _log.LogInformation("Loading images for series {mediaId}...", mediaId);
          return await GetImagesForMedia(mediaId, cancellationToken);

        case Movie:
          _log.LogInformation("Loading images for movie {mediaId}...", mediaId);
          return await GetImagesForMedia(mediaId, cancellationToken);

        case Book:
          _log.LogInformation("Loading images for book {mediaId}...", mediaId);
          if (Name == ProviderNames.HikkaNovel)
            return await GetImagesForNovel(mediaId, cancellationToken);
          if (Name == ProviderNames.HikkaManga)
            return await GetImagesForManga(mediaId, cancellationToken);
          break;
      }
    }

    return [];
  }

  private async Task<IEnumerable<RemoteImageInfo>> GetImagesForMedia(string mediaId, CancellationToken cancellationToken)
  {
    var list = new List<RemoteImageInfo>();
    var media = await _hikkaApi.GetAnimeAsync(mediaId, cancellationToken).ConfigureAwait(false);

    if (!string.IsNullOrEmpty(media.Image))
    {
      list.Add(new RemoteImageInfo
      {
        ProviderName = Name,
        Type = ImageType.Primary,
        Url = media.Image
      });
    }
    return list;
  }

  protected async Task<IEnumerable<RemoteImageInfo>> GetImagesForNovel(string mediaId, CancellationToken cancellationToken)
  {
    var list = new List<RemoteImageInfo>();
    var media = await _hikkaApi.GetNovelAsync(mediaId, cancellationToken).ConfigureAwait(false);

    if (!string.IsNullOrEmpty(media.Image))
    {
      list.Add(new RemoteImageInfo
      {
        ProviderName = Name,
        Type = ImageType.Primary,
        Url = media.Image
      });
    }
    return list;
  }

  protected async Task<IEnumerable<RemoteImageInfo>> GetImagesForManga(string mediaId, CancellationToken cancellationToken)
  {
    var list = new List<RemoteImageInfo>();
    var media = await _hikkaApi.GetMangaAsync(mediaId, cancellationToken).ConfigureAwait(false);

    if (!string.IsNullOrEmpty(media.Image))
    {
      list.Add(new RemoteImageInfo
      {
        ProviderName = Name,
        Type = ImageType.Primary,
        Url = media.Image
      });
    }
    return list;
  }

  public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
  {
    return [ImageType.Primary];
  }

  public bool Supports(BaseItem item) => item is Series || item is Season || item is Movie || item is Book;
}
