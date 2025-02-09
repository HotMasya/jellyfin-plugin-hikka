using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.PeopleProviders;

public class HikkaStaffImageProvider : IRemoteImageProvider
{
    private readonly ILogger _log;

    private readonly HikkaApi _hikkaApi;

    public HikkaStaffImageProvider(ILogger<HikkaStaffImageProvider> logger)
    {
        _log = logger;
        _hikkaApi = new HikkaApi();
    }

    public string Name { get; protected set; } = ProviderNames.HikkaPeople;

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
    {
        var personId = item.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(personId))
        {
            _log.LogInformation("Loading images for person {PersonId}...", personId);
            return await GetImagesForPerson(personId, cancellationToken).ConfigureAwait(false);
        }

        return [];
    }

    private async Task<IEnumerable<RemoteImageInfo>> GetImagesForPerson(string mediaId, CancellationToken cancellationToken)
    {
        var list = new List<RemoteImageInfo>();
        var media = await _hikkaApi.GetPersonAsync(mediaId, cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(media.Image))
        {
            list.Add(new RemoteImageInfo
            {
                ProviderName = Name,
                Type = ImageType.Primary,
                Url = SearchHelpers.PreprocessImageUrl(media.Image)
            });
        }

        return list;
    }

    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return [ImageType.Primary];
    }

    public bool Supports(BaseItem item) => item is Person;
}
