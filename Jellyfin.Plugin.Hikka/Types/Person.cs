using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Model.Providers;
using JellyfinPerson = MediaBrowser.Controller.Entities.Person;

namespace Jellyfin.Plugin.Hikka.Types;

public class Person
{
    public string? NameNative { get; set; }

    public string? NameUa { get; set; }

    public string? NameEn { get; set; }

    public string? Image { get; set; }

    public required string Slug { get; set; }

    public string? DescriptionUa { get; set; }

    public required IEnumerable<string> Synonyms { get; set; }

    public string? GetPreferredName()
    {
        return LanguageUtils.GetPreferredStringValue(NameUa, NameEn);
    }

    public JellyfinPerson ToPerson(string providerName)
    {
        return new JellyfinPerson
        {
            Name = GetPreferredName(),
            OriginalTitle = NameNative,
            Overview = DescriptionUa,
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }

    public RemoteSearchResult ToSearchResult(string providerName)
    {
        return new RemoteSearchResult
        {
            Name = GetPreferredName(),
            ImageUrl = SearchHelpers.PreprocessImageUrl(Image),
            SearchProviderName = providerName,
            ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
        };
    }
}
