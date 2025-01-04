using System.Net.Http.Headers;
using Jellyfin.Plugin.Hikka.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Hikka;

public class Plugin : BasePlugin<PluginConfiguration>
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public static Plugin Instance { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
        httpClient.DefaultRequestHeaders.UserAgent.Add(
        new ProductInfoHeaderValue(Name, Version.ToString()));

        return httpClient;
    }
}
