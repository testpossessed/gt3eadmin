using System;
using System.Diagnostics;
using System.IO;

namespace GT3e.Admin.Services;

public static class PathProvider
{
    private const string AccFolderName = "Assetto Corsa Competizione";
    private const string AccConfigFolderName = "Config";
    private const string AccReplaysFolderName = "Replay";
    private const string AccReplaySavedFolderName = "Saved";
    private const string AccResultsFolderName = "Results";
    private const string AccSetupsFolderName = "Setups";
    public const string AppSettingsFileName = "appsettings.json";
    private const string UserSettingsFileName = "userSettings.json";
    private const string BroadcastingSettingsFileName = "broadcasting.json";
    private const string SeasonSettingsFileName = "seasonEntity.json";
    private const string PackageDownloadFolderName = "Downloads";
    private const string VerificationTestPackagesFolderName = "VerificationTestPackages";

    static PathProvider()
    {
        var processName = Process.GetCurrentProcess()
                                 .ProcessName;
        var localAppDataFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        AppDataFolderPath = Path.Combine(localAppDataFolderPath, processName);
        AppSettingsFilePath = Path.Combine(AppDataFolderPath, AppSettingsFileName);
        var executionFolder = AppDomain.CurrentDomain.BaseDirectory;
        AppFolderPath = Path.GetDirectoryName(executionFolder)!;
        DefaultSettingsFilePath = Path.Combine(AppFolderPath!, AppSettingsFileName);
        var myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        AccSetupsFolderPath = Path.Combine(myDocumentsFolderPath, AccFolderName, AccSetupsFolderName);
        UserSettingsFilePath = Path.Combine(AppDataFolderPath, UserSettingsFileName);
        AccConfigFolderPath = Path.Combine(myDocumentsFolderPath, AccFolderName, AccConfigFolderName);
        AccBroadcastingSettingsFilePath = Path.Combine(AccConfigFolderPath, BroadcastingSettingsFileName);
        AccSeasonSettingsFilePath =
            Path.Combine(myDocumentsFolderPath, AccConfigFolderPath, SeasonSettingsFileName);
        AccResultFolderPath = Path.Combine(myDocumentsFolderPath, AccFolderName, AccResultsFolderName);
        AccSavedReplaysFolderPath = Path.Combine(myDocumentsFolderPath,
            AccFolderName,
            AccReplaysFolderName,
            AccReplaySavedFolderName);
        DownloadsFolderPath = Path.Combine(AppDataFolderPath, PackageDownloadFolderName);
        VerificationTestsPackageFolderPath = Path.Combine(AppDataFolderPath,
            PackageDownloadFolderName,
            VerificationTestPackagesFolderName);
    }

    public static string DownloadsFolderPath { get; }
    public static string AccBroadcastingSettingsFilePath { get; }
    public static string AccConfigFolderPath { get; }
    public static string AccSetupsFolderPath { get; }
    public static string AppDataFolderPath { get; }
    public static string AppFolderPath { get; }
    public static string AppSettingsFilePath { get; }
    public static string DefaultSettingsFilePath { get; }
    public static string UserSettingsFilePath { get; }
    public static string VerificationTestsPackageFolderPath { get; }
    public static string AccResultFolderPath { get; set; }
    public static string AccSavedReplaysFolderPath { get; set; }
    public static string AccSeasonSettingsFilePath { get; set; }

    public static string GetLastFolderName(string path)
    {
        var folderPath = Path.HasExtension(path)? Path.GetDirectoryName(path): path;
        return new DirectoryInfo(folderPath!).Name;
    }
}