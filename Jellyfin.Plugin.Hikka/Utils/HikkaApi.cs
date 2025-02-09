using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Utils;

public delegate Task<PaginationResponse<T>> GetAllItemsQuery<T>(string slug, PaginationQuery query, CancellationToken cancellationToken);

public class HikkaApi
{
    private const string BaseUrl = "https://api.hikka.io";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private async Task<IEnumerable<TData>> GetAllDataAsync<TData>(GetAllItemsQuery<TData> getAllItemsQuery, string slug, CancellationToken cancellationToken)
    {
        List<TData> data = [];

        var query = new PaginationQuery
        {
            Page = 1,
            Size = 100,
        };

        var searchResult = await getAllItemsQuery(slug, query, cancellationToken).ConfigureAwait(false);

        if (searchResult.Pagination.Total > 0)
        {
            data.AddRange(searchResult.List);
        }

        while (searchResult.Pagination.Page < searchResult.Pagination.Total)
        {
            query.Page++;
            searchResult = await getAllItemsQuery(slug, query, cancellationToken).ConfigureAwait(false);
            data.AddRange(searchResult.List);
        }

        return data;
    }

    public async Task<PaginationResponse<AnimeSearchResult>> SearchAnimeAsync(AnimeSearchArgs args, CancellationToken cancellationToken)
    {
        return await MakePostRequestAsync<PaginationResponse<AnimeSearchResult>, AnimeSearchArgs>("/anime", args, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Anime> GetAnimeAsync(string slug, CancellationToken cancellationToken)
    {
        return await MakeGetRequestAsync<Anime>($"/anime/{slug}", cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginationResponse<Episode>> GetAnimeEpisodesAsync(string slug, PaginationQuery? query, CancellationToken cancellationToken)
    {
        string queryString = string.Empty;

        if (query != null)
        {
            queryString = $"size={query.Size}&page={query.Page}";
        }

        return await MakeGetRequestAsync<PaginationResponse<Episode>>($"/anime/{slug}/episodes?{queryString}", cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Episode>> GetAllAnimeEpisodesAsync(string slug, CancellationToken cancellationToken)
    {
        return await GetAllDataAsync(GetAnimeEpisodesAsync, slug, cancellationToken).ConfigureAwait(false);
        // List<Episode> episodes = [];

        // var query = new PaginationQuery
        // {
        //     Page = 1,
        //     Size = 100,
        // };

        // var searchResult = await GetAnimeEpisodesAsync(slug, query, cancellationToken).ConfigureAwait(false);

        // if (searchResult.Pagination.Total > 0)
        // {
        //     episodes.AddRange(searchResult.List);
        // }

        // while (searchResult.Pagination.Page < searchResult.Pagination.Total)
        // {
        //     query.Page++;
        //     searchResult = await GetAnimeEpisodesAsync(slug, query, cancellationToken).ConfigureAwait(false);
        //     episodes.AddRange(searchResult.List);
        // }

        // return episodes;
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

    public async Task<PaginationResponse<Person>> SearchPeopleAsync(SearchArgsBase args, CancellationToken cancellationToken)
    {
        return await MakePostRequestAsync<PaginationResponse<Person>, SearchArgsBase>("/people", args, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Person> GetPersonAsync(string slug, CancellationToken cancellationToken)
    {
        return await MakeGetRequestAsync<Person>($"/people/{slug}", cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginationResponse<StaffMember>> GetAnimeStaffMembers(string slug, PaginationQuery? query, CancellationToken cancellationToken)
    {
        string queryString = string.Empty;

        if (query != null)
        {
            queryString = $"size={query.Size}&page={query.Page}";
        }

        return await MakeGetRequestAsync<PaginationResponse<StaffMember>>($"/anime/{slug}/staff?{queryString}", cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<StaffMember>> GetAllAnimeStaffMembers(string slug, CancellationToken cancellationToken)
    {
        return await GetAllDataAsync(GetAnimeStaffMembers, slug, cancellationToken).ConfigureAwait(false);
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
