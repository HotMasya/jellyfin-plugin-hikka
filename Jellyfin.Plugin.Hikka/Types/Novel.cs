
using Jellyfin.Plugin.Hikka.Types.Abstract;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Types;

public class Novel : MediaBase, IBookConvertable
{
  public Author[] Authors { get; set; }
  public Magazine[] Magazines { get; set; }
  public External[] External { get; set; }
  public string TitleOriginal { get; set; }
  // public Stats Stats { get; set; }
  public int? Chapters { get; set; }
  public string[] Synonyms { get; set; }
  public int CommentsCount { get; set; }
  public int? Volumes { get; set; }

  public Book ToBook(string providerName)
  {
    return new Book
    {
      Name = TitleUa,
      OriginalTitle = TitleOriginal,
      Overview = SynopsisUa,
      ProductionYear = Year,
      PremiereDate = GetDate(StartDate),
      EndDate = GetDate(EndDate),
      CommunityRating = Score,
      Genres = [.. GetGenres()],
      // TODO: Do something with tags
      Tags = [],
      ProviderIds = new Dictionary<string, string> { { providerName, Slug } }
    };
  }
}
