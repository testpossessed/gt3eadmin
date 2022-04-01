namespace GT3e.Admin.Models;

public class VerificationTestPackageInfo
{
    public VerificationTestPackageInfo(string name, string url)
    {
        this.Name = name;
        this.Url = url;
    }

    public string Name { get; set; }
    public string Url { get; set; }
}