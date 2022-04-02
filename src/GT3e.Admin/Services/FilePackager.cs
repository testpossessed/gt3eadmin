using System.IO;
using System.IO.Compression;
using System.Threading;

namespace GT3e.Admin.Services;

internal class FilePackager
{
    internal static string PackageVerificationTestFiles(string steamId,
        string resultFilePath,
        string replayFilePath)
    {
        var zipFilePath = Path.Combine(PathProvider.AppDataFolderPath, $"{steamId}.zip");
        using var zipToOpen = new FileStream(zipFilePath, FileMode.Create);
        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        archive.CreateEntryFromFile(resultFilePath, $"{steamId}.json");
        Thread.Sleep(1000);
        archive.CreateEntryFromFile(replayFilePath, $"{steamId}.rpy");

        return zipFilePath;
    }

    internal static void Unpack(string packagePath, string targetFolder)
    {
        var message = $"Unpacking {packagePath} to {targetFolder}";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");
        using var zipToOpen = new FileStream(packagePath, FileMode.Open);
        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);
        archive.ExtractToDirectory(targetFolder, true);
        message = $"Unpacked {packagePath} to {targetFolder}";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}");
    }

    internal static void UnpackToSameLocation(string packagePath)
    {
        var targetFolder = Path.GetDirectoryName(packagePath)!;
        Unpack(packagePath, targetFolder);
    }
}