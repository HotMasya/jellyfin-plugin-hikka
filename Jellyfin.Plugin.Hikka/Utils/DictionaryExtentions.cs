namespace Jellyfin.Plugin.Hikka.Utils;

public static class DictionaryExtensions
{
  public static T GetOrDefault<TKey, T>(this IDictionary<TKey, T> dict, TKey key)
  {
    if (dict.TryGetValue(key, out T value))
      return value;

    return default;
  }
}
