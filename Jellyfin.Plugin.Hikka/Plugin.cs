using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Common.Net;
using System.Net.Http.Headers;

using Jellyfin.Plugin.Hikka.Configuration;

namespace Jellyfin.Plugin.Hikka;

public class Plugin : BasePlugin<PluginConfiguration>
{
  private IHttpClientFactory _httpClientFactory;

  public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, IHttpClientFactory httpClientFactory) : base(applicationPaths, xmlSerializer)
  {
    Instance = this;
    _httpClientFactory = httpClientFactory;
  }

  public HttpClient GetHttpClient()
  {
    var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
    httpClient.DefaultRequestHeaders.UserAgent.Add(
    new ProductInfoHeaderValue(Name, Version.ToString()));

    return httpClient;
  }

  public override string Name => Constants.PluginName;
    public override string Description => Constants.PluginDescription;
    public override Guid Id => Guid.Parse(Constants.PluginGuid);

  public static Plugin Instance { get; private set; }
}
