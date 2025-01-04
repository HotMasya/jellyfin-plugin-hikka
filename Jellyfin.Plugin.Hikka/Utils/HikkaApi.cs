using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;

using Jellyfin.Plugin.Hikka.Types;

namespace Jellyfin.Plugin.Hikka.Utils;

public class HikkaApi
{
  private const string BaseUrl = "https://api.hikka.io";

  private static readonly JsonSerializerOptions jsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public async Task<PaginationResponse<AnimeSearchResult>> SearchAnimeAsync(AnimeSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<AnimeSearchResult>, AnimeSearchArgs>("/anime", args, cancellationToken);
  }

  public async Task<Anime> GetAnimeAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Anime>($"/anime/{slug}", cancellationToken);
  }

  public async Task<PaginationResponse<MangaSearchResult>> SearchMangaAsync(MangaSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<MangaSearchResult>, MangaSearchArgs>("/manga", args, cancellationToken);
  }

  public async Task<Manga> GetMangaAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Manga>($"/manga/{slug}", cancellationToken);
  }

  public async Task<PaginationResponse<NovelSearchResult>> SearchNovelAsync(NovelSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<NovelSearchResult>, NovelSearchArgs>("/novel", args, cancellationToken);
  }

  public async Task<Novel> GetNovelAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Novel>($"/novel/{slug}", cancellationToken);
  }

  private async Task<TResponse> MakeGetRequestAsync<TResponse>(string path, CancellationToken cancellationToken)
  {
    var httpClient = Plugin.Instance.GetHttpClient();

    using var response = await httpClient.GetAsync(BaseUrl + path, cancellationToken).ConfigureAwait(false);

    if (!response.IsSuccessStatusCode)
    {
      return default;
    }

    using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, jsonOptions, cancellationToken).ConfigureAwait(false);
  }

  private async Task<TResponse> MakePostRequestAsync<TResponse, TArgs>(string path, TArgs args, CancellationToken cancellationToken)
  {
    var httpClient = Plugin.Instance.GetHttpClient();

    using var content = new StringContent(JsonSerializer.Serialize(args, jsonOptions), Encoding.UTF8, "application/json");
    using var response = await httpClient.PostAsync(BaseUrl + path, content, cancellationToken).ConfigureAwait(false);

    if (!response.IsSuccessStatusCode)
    {
      return default;
    }

    using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, jsonOptions, cancellationToken).ConfigureAwait(false);
  }
}
