namespace Jellyfin.Plugin.Hikka.Utils;

public static class DictionaryExtentions
{
    public static T GetOrDefault<TKey, T>(this IDictionary<TKey, T> dict, TKey key)
    {
        if (dict.TryGetValue(key, out var value))
        {
            return value;
        }

        return default!;
    }
}
