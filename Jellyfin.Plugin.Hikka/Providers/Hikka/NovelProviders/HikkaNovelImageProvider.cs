using System.Net.Http.Headers;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.MangaProviders;

public class HikkaNovelImageProvider : IRemoteImageProvider
{
    private readonly ILogger _log;

    private readonly HikkaApi _hikkaApi;

    public HikkaNovelImageProvider(ILogger<HikkaNovelImageProvider> logger)
    {
        _log = logger;
        _hikkaApi = new HikkaApi();
    }

    public string Name { get; protected set; } = ProviderNames.HikkaNovel;

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

    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
    {
        var mediaId = item.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(mediaId))
        {
            _log.LogInformation("Loading images for book {MediaId}...", mediaId);
            return await GetImagesForNovel(mediaId, cancellationToken).ConfigureAwait(false);
        }

        return [];
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

    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return [ImageType.Primary];
    }

    public bool Supports(BaseItem item) => item is Book;
}
