using System;
using System.Diagnostics;
using System.IO;

namespace GT3e.Admin.Services;

public static class PathProvider
{
    public const string AppSettingsFileName = "appsettings.json";
    private const string UserSettingsFileName = "userSettings.json";
    private const string DownloadFolderName = "Downloads";
    private const string VerificationTestPackagesFolderName = "VerificationTestPackages";
    private const string AppDocumentsFolderName = "GT3e.Admin";

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
        UserSettingsFilePath = Path.Combine(AppDataFolderPath, UserSettingsFileName);
        DownloadsFolderPath = Path.Combine(AppDataFolderPath, DownloadFolderName);
        VerificationTestsPackageFolderPath = Path.Combine(AppDataFolderPath,
            DownloadFolderName,
            VerificationTestPackagesFolderName);
        AppDocumentsFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                AppDocumentsFolderName);
        AppDocumentsDownloadFolderPath = Path.Combine(AppDocumentsFolderPath, DownloadFolderName);
    }

    public static string AppDataFolderPath { get; }
    public static string AppDocumentsDownloadFolderPath { get; }
    public static string AppDocumentsFolderPath { get; }
    public static string AppFolderPath { get; }
    public static string AppSettingsFilePath { get; }
    public static string DefaultSettingsFilePath { get; }
    public static string DownloadsFolderPath { get; }
    public static string UserSettingsFilePath { get; }
    public static string VerificationTestsPackageFolderPath { get; }
}