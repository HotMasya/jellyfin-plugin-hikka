namespace Jellyfin.Plugin.Hikka.Types;

public class Person
{
    public string DataType { get; set; }
    public string NameNative { get; set; }
    public string NameUa { get; set; }
    public string NameEn { get; set; }
    public string Image { get; set; }
    public string Slug { get; set; }
    public string DescriptionUa { get; set; }
    public string[] Synonyms { get; set; }
}
