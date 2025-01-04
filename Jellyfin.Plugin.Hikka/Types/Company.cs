namespace Jellyfin.Plugin.Hikka.Types;

public class Company {
  public string Image { get; set; }
  public string Slug { get; set; }
  public string Name { get; set; }
}

public class CompanyInfo {
  public Company Company { get; set; }
  public string Type { get; set; }
}
