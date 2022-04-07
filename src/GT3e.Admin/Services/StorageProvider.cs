using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GT3e.Admin.Models;
using Newtonsoft.Json;

namespace GT3e.Admin.Services;

internal class StorageProvider
{
    internal static Task<IEnumerable<VerificationTestPackageInfo>> GetPendingVerificationTests()
    {
        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "verification-tests");
        var pages = blobContainerClient.GetBlobs().AsPages();

        return Task.FromResult<IEnumerable<VerificationTestPackageInfo>>((from page in pages from item in page.Values select new VerificationTestPackageInfo(item.Name)).ToList());
    }

    internal static async Task<IEnumerable<DriverStats>> GetDriverStats()
    {
        var result = new List<DriverStats>();
        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "driver-stats");
        var pages = blobContainerClient.GetBlobs()
                                       .AsPages();

        foreach(var page in pages)
        {
            foreach(var item in page.Values)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);
                var blob = await blobClient.DownloadAsync();
                using var reader = new StreamReader(blob.Value.Content);
                var json = await reader.ReadToEndAsync();
                var driverStats = JsonConvert.DeserializeObject<DriverStats>(json);
                result.Add(driverStats);
            }
        }

        return result;
    }

    internal static async Task<string> GetPendingVerificationTest(VerificationTestPackageInfo testInfo)
    {
        var message = $"Downloading {testInfo.FileName}";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");

        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "verification-tests");

        var blobClient = blobContainerClient.GetBlobClient(testInfo.FileName);
        var packageDownloadFolderPath =
            Path.Combine(PathProvider.VerificationTestsPackageFolderPath, testInfo.Name);
        var packageDownloadFilePath = Path.Combine(packageDownloadFolderPath, testInfo.FileName);

        if(File.Exists(packageDownloadFilePath))
        {
            File.Delete(packageDownloadFilePath);
        }

        if(!Directory.Exists(packageDownloadFolderPath))
        {
            Directory.CreateDirectory(packageDownloadFolderPath);
        }


        var progressHandler = new Progress<long>();
        var lastProgress = 0D;
        var totalBytes = (await blobClient.GetPropertiesAsync()).Value.ContentLength;
        progressHandler.ProgressChanged += (sender, bytesUploaded) =>
                                           {
                                               if(bytesUploaded <= 0)
                                               {
                                                   return;
                                               }

                                               var progress =
                                                   Math.Floor((double) bytesUploaded / totalBytes * 100);
                                               if(progress > lastProgress && progress % 10 == 0)
                                               {
                                                   ConsoleLog.Write($"Downloaded {progress}%...");
                                               }

                                               lastProgress = progress;
                                           };

        var options = new BlobDownloadToOptions
        {
            ProgressHandler = progressHandler,
            TransferOptions = new StorageTransferOptions
            {
                MaximumTransferSize = 4 * 1024 * 1024,
                InitialTransferSize = 4 * 1024 * 1024
            }

        };
        await blobClient.DownloadToAsync(packageDownloadFilePath, options, CancellationToken.None);
        message = $"Package downloaded to {packageDownloadFolderPath}";
        LogWriter.Info(message);
        ConsoleLog.Write(message);

        return packageDownloadFilePath;
    }

    internal static async Task DeletePendingVerificationTest(VerificationTestPackageInfo testInfo)
    {
        var message = $"Deleting {testInfo.FileName}";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");

        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "verification-tests");

        var blobClient = blobContainerClient.GetBlobClient(testInfo.FileName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        message = $"Deleted {testInfo.FileName}";
        LogWriter.Info(message);
        ConsoleLog.Write(message);
    }

    internal static async Task<DriverStats?> GetDriverStats(string steamId)
    {
        var message = $"Downloading any existing driver stats";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");


        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "driver-stats");
        await blobContainerClient.CreateIfNotExistsAsync();

        var blobClient = blobContainerClient.GetBlobClient($"{steamId}.json");
        var exists = await blobClient.ExistsAsync();
        if(!exists.Value)
        {
            return null;
        }

        var progressHandler = new Progress<long>();
        var lastProgress = 0D;
        var totalBytes = (await blobClient.GetPropertiesAsync()).Value.ContentLength;
        progressHandler.ProgressChanged += (sender, bytesUploaded) =>
                                           {
                                               if(bytesUploaded <= 0)
                                               {
                                                   return;
                                               }

                                               var progress =
                                                   Math.Floor((double) bytesUploaded / totalBytes * 100);
                                               if(progress > lastProgress && progress % 10 == 0)
                                               {
                                                   ConsoleLog.Write($"Downloaded {progress}%...");
                                               }

                                               lastProgress = progress;
                                           };
        
        var statsBlob = await blobClient.DownloadContentAsync(progressHandler: progressHandler);
        var jsonBytes = statsBlob.Value.Content.ToArray();
        var json = Encoding.UTF8.GetString(jsonBytes);
        var driverStats = JsonConvert.DeserializeObject<DriverStats>(json);

        message = $"Downloaded stats";
        LogWriter.Info(message);
        ConsoleLog.Write(message);

        return driverStats;
    }

    internal static async Task UploadDriverStats(DriverStats driverStats)
    {
        var message = $"Uploading driver stats";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");

        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "driver-stats");
        await blobContainerClient.CreateIfNotExistsAsync();

        var blobClient = blobContainerClient.GetBlobClient($"{driverStats.SteamId}.json");
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

        var json = JsonConvert.SerializeObject(driverStats);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        await using var stream = new MemoryStream(jsonBytes);
        var totalBytes = jsonBytes.Length;
        var progressHandler = new Progress<long>();
        var lastProgress = 0D;
        progressHandler.ProgressChanged += (sender, bytesUploaded) =>
                                           {
                                               if(bytesUploaded <= 0)
                                               {
                                                   return;
                                               }

                                               var progress =
                                                   Math.Floor((double) bytesUploaded / totalBytes * 100);
                                               if(progress > lastProgress && progress % 10 == 0)
                                               {
                                                   ConsoleLog.Write($"Uploaded {progress}%...");
                                               }

                                               lastProgress = progress;
                                           };

        var options = new BlobUploadOptions
        {
            ProgressHandler = progressHandler
        };
        await blobClient.UploadAsync(stream, options, CancellationToken.None);
        message = $"Uploaded driver stats";
        LogWriter.Info(message);
        ConsoleLog.Write(message);
    }

    public static async Task DownloadCustomSkin(CustomSkinInfo customSkin)
    {
        var message = $"Downloading {customSkin.FileName}";
        LogWriter.Info(message);
        ConsoleLog.Write($"{message}...");

        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "custom-skins");

        var blobClient = blobContainerClient.GetBlobClient(customSkin.FileName);
      
        var packageDownloadFilePath = Path.Combine(PathProvider.AppDocumentsDownloadFolderPath, customSkin.FileName);

        if (File.Exists(packageDownloadFilePath))
        {
            File.Delete(packageDownloadFilePath);
        }


        var progressHandler = new Progress<long>();
        var lastProgress = 0D;
        var totalBytes = (await blobClient.GetPropertiesAsync()).Value.ContentLength;
        progressHandler.ProgressChanged += (sender, bytesUploaded) =>
        {
            if (bytesUploaded <= 0)
            {
                return;
            }

            var progress =
                Math.Floor((double)bytesUploaded / totalBytes * 100);
            if (progress > lastProgress && progress % 10 == 0)
            {
                ConsoleLog.Write($"Downloaded {progress}%...");
            }

            lastProgress = progress;
        };

        var options = new BlobDownloadToOptions
        {
            ProgressHandler = progressHandler,
            TransferOptions = new StorageTransferOptions
            {
                MaximumTransferSize = 4 * 1024 * 1024,
                InitialTransferSize = 4 * 1024 * 1024
            }

        };
        await blobClient.DownloadToAsync(packageDownloadFilePath, options, CancellationToken.None);
        message = $"Skin package downloaded to {packageDownloadFilePath}";
        LogWriter.Info(message);
        ConsoleLog.Write(message);
    }

    public static Task<IEnumerable<CustomSkinInfo>> GetCustomSkins()
    {
        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "custom-skins");
        var pages = blobContainerClient.GetBlobs()
                                       .AsPages();

        return Task.FromResult<IEnumerable<CustomSkinInfo>>(
            (from page in pages from item in page.Values select new CustomSkinInfo(item.Name))
            .ToList());
    }
}