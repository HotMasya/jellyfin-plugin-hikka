using Jellyfin.Plugin.Hikka.Utils;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Types;

public class StaffMember
{
    public required IEnumerable<StaffMemberRole> Roles { get; set; }

    public required Person Person { get; set; }

    public int? Weight { get; set; }

    public PersonInfo ToPersonInfo(string providerName)
    {
        return new PersonInfo
        {
            Name = Person.GetPreferredName(),
            Role = Roles.FirstOrDefault()?.GetPreferredName(),
            ImageUrl = SearchHelpers.PreprocessImageUrl(Person.Image),
            ProviderIds = new Dictionary<string, string> { { providerName, Person.Slug } }
        };
    }
}
