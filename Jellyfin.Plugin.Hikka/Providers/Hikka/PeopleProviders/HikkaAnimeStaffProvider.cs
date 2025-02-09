using Jellyfin.Plugin.Hikka.Types;
using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

using JellyfinPerson = MediaBrowser.Controller.Entities.Person;

namespace Jellyfin.Plugin.Hikka.Providers.Hikka.PeopleProviders;

public class HikkaAnimeStaffProvider : IRemoteMetadataProvider<JellyfinPerson, PersonLookupInfo>, IHasOrder
{
    private readonly ILogger _log;
    private readonly HikkaApi _hikkaApi;

    public HikkaAnimeStaffProvider(ILogger<HikkaAnimeStaffProvider> logger)
    {
        _log = logger;
        _hikkaApi = new HikkaApi();
    }

    public string Name => ProviderNames.HikkaPeople;

    public int Order => -2;

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<MetadataResult<JellyfinPerson>> GetMetadata(PersonLookupInfo info, CancellationToken cancellationToken)
    {
        var result = new MetadataResult<JellyfinPerson>();
        Person? person = null;
        var personId = info.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(personId))
        {
            _log.LogInformation("Person id \"{PersonId}\" found. Loading metadata.", personId);
            person = await _hikkaApi.GetPersonAsync(personId, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var searchName = SearchHelpers.PreprocessTitle(Name);

            _log.LogInformation("Searching for {SearchName}...", searchName);
            var searchResults = await _hikkaApi.SearchPeopleAsync(new AnimeSearchArgs { Query = searchName }, cancellationToken).ConfigureAwait(false);

            if (searchResults.Pagination.Total > 0)
            {
                var primaryResult = searchResults.List.First();
                _log.LogInformation("Found person metadata for \"{ResultName}\"", primaryResult.GetPreferredName());
                person = await _hikkaApi.GetPersonAsync(primaryResult.Slug, cancellationToken).ConfigureAwait(false);
            }
        }

        if (person != null)
        {
            result.HasMetadata = true;
            result.Item = person.ToPerson(Name);
            result.Provider = Name;
        }

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(PersonLookupInfo searchInfo, CancellationToken cancellationToken)
    {
        var results = new List<RemoteSearchResult>();

        var personId = searchInfo.ProviderIds.GetOrDefault(Name);

        if (!string.IsNullOrEmpty(personId))
        {
            _log.LogInformation("Person id \"{PersonId}\" found. Loading metadata.", personId);
            var person = await _hikkaApi.GetPersonAsync(personId, cancellationToken).ConfigureAwait(false);

            if (person != null)
            {
                results.Add(person.ToSearchResult(Name));
            }
        }

        if (!string.IsNullOrEmpty(searchInfo.Name))
        {
            _log.LogInformation("Searching for {SearchName}...", searchInfo.Name);
            var searchResults = await _hikkaApi.SearchPeopleAsync(new AnimeSearchArgs { Query = searchInfo.Name }, cancellationToken).ConfigureAwait(false);
            _log.LogInformation("Found {Count} results", searchResults.Pagination.Total);

            if (searchResults.Pagination.Total > 0)
            {
                results.AddRange(searchResults.List.Select((list) => list.ToSearchResult(Name)).ToArray());
            }
        }

        return results;
    }
}
