using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.Hikka.Utils;

public static class SearchHelpers
{
    public static string PreprocessTitle(string path)
    {
        string input = path;

        // Season designation
        input = Regex.Replace(input, @"(\s|\.)S[0-9]{1,2}", string.Empty);
        // ~ ALT NAME ~
        input = Regex.Replace(input, @"\s*~(\w|[0-9]|\s)+~", string.Empty);

        // Native Name (English Name)
        // Only replaces if the name ends with a parenthesis to hopefully avoid mangling titles with parens (e.g. Evangelion 1.11 You Are (Not) Alone)
        input = Regex.Replace(input.Trim(), @"\((\w|[0-9]|\s)+\)$", string.Empty);

        // Replace & with "and" to avoid lookup failures
        input = Regex.Replace(input, @"\s?&\s?", " and ");

        // Replace the following characters with a space, to avoid failed lookups
        input = Regex.Replace(input, @"\#", " ");

        // Truncate suggested Jellyfin folder format for the anilist search. Example: The Melancholy of Haruhi Suzumiya (2006) [tvdbid-79414]
        input = Regex.Replace(input.Trim(), @"\([0-9]{4}\)\s*\[(\w|[0-9]|-)+\]$", string.Empty);

        return input;
    }

    public static string? PreprocessImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        return Constants.ImageProxyUrl + url;
    }
}
