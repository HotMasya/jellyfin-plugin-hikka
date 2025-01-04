using System.Globalization;
using System.Net.Http.Headers;
using Jellyfin.Plugin.Hikka.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Hikka;

public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private IHttpClientFactory _httpClientFactory;

    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, IHttpClientFactory httpClientFactory) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
        _httpClientFactory = httpClientFactory;
    }

    public override string Name => Constants.PluginName;

    public override string Description => Constants.PluginDescription;

    public override Guid Id => Guid.Parse(Constants.PluginGuid);

    public static Plugin? Instance { get; private set; }

    public HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
        httpClient.DefaultRequestHeaders.UserAgent.Add(
        new ProductInfoHeaderValue(Name, Version.ToString()));

        return httpClient;
    }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.configPage.html", GetType().Namespace)
            }
        ];
    }
}
