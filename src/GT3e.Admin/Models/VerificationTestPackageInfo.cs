using System;
using System.IO;

namespace GT3e.Admin.Models;

public class VerificationTestPackageInfo
{
    public VerificationTestPackageInfo(string fileName)
    {
        this.Name = fileName.Substring(0, fileName.LastIndexOf(".", StringComparison.Ordinal));
        this.FileName = fileName;
    }

    public string Name { get; set; }
    public string FileName { get; set; }
}