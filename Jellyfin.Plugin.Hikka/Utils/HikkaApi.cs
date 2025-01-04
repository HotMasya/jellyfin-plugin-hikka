using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types;

namespace Jellyfin.Plugin.Hikka.Utils;

public class HikkaApi
{
  private const string BaseUrl = "https://api.hikka.io";

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public async Task<PaginationResponse<AnimeSearchResult>> SearchAnimeAsync(AnimeSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<AnimeSearchResult>, AnimeSearchArgs>("/anime", args, cancellationToken).ConfigureAwait(false);
  }

  public async Task<Anime> GetAnimeAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Anime>($"/anime/{slug}", cancellationToken).ConfigureAwait(false);
  }

  public async Task<PaginationResponse<MangaSearchResult>> SearchMangaAsync(MangaSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<MangaSearchResult>, MangaSearchArgs>("/manga", args, cancellationToken).ConfigureAwait(false);
  }

  public async Task<Manga> GetMangaAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Manga>($"/manga/{slug}", cancellationToken).ConfigureAwait(false);
  }

  public async Task<PaginationResponse<NovelSearchResult>> SearchNovelAsync(NovelSearchArgs args, CancellationToken cancellationToken)
  {
    return await MakePostRequestAsync<PaginationResponse<NovelSearchResult>, NovelSearchArgs>("/novel", args, cancellationToken).ConfigureAwait(false);
  }

  public async Task<Novel> GetNovelAsync(string slug, CancellationToken cancellationToken)
  {
    return await MakeGetRequestAsync<Novel>($"/novel/{slug}", cancellationToken).ConfigureAwait(false);
  }

  private async Task<TResponse> MakeGetRequestAsync<TResponse>(string path, CancellationToken cancellationToken)
  {
    var httpClient = Plugin.Instance!.GetHttpClient();

    using var response = await httpClient.GetAsync(BaseUrl + path, cancellationToken).ConfigureAwait(false);

    if (!response.IsSuccessStatusCode)
    {
      throw new HttpRequestException($"Server return response code: {response.StatusCode}");
    }

    using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    var result = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, JsonOptions, cancellationToken).ConfigureAwait(false);

    if (result == null)
    {
      throw new HttpRequestException($"Server returned empty response");
    }

    return result;
  }

  private async Task<TResponse> MakePostRequestAsync<TResponse, TArgs>(string path, TArgs args, CancellationToken cancellationToken)
  {
    var httpClient = Plugin.Instance!.GetHttpClient();

    using var content = new StringContent(JsonSerializer.Serialize(args, JsonOptions), Encoding.UTF8, "application/json");
    using var response = await httpClient.PostAsync(BaseUrl + path, content, cancellationToken).ConfigureAwait(false);

    if (!response.IsSuccessStatusCode)
    {
      throw new HttpRequestException($"Server return response code: {response.StatusCode}");
    }

    using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    var result = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, JsonOptions, cancellationToken).ConfigureAwait(false);

    if (result == null)
    {
      throw new HttpRequestException($"Server returned empty response");
    }

    return result;
  }
}
