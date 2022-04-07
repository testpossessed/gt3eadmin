namespace GT3e.Admin.Models;

public class CustomSkinInfo
{
    public CustomSkinInfo(string name)
    {
        this.Name = name.Replace(".zip", "");
        this.FileName = name;
    }

    public string FileName { get; }
    public string Name { get; }
}