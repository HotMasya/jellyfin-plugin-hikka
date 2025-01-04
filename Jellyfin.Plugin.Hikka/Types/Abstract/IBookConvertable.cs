using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.Hikka.Types.Abstract;

public interface IBookConvertable
{
    public Book ToBook(string providerName);
}
