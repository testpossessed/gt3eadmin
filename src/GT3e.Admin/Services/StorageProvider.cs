using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GT3e.Admin.Models;

namespace GT3e.Admin.Services;

internal class StorageProvider
{
    internal static IEnumerable<VerificationTestPackageInfo> GetPendingVerificationTests()
    {
        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "verification-tests");
        var pages = blobContainerClient.GetBlobs().AsPages();

        return (from page in pages from item in page.Values select new VerificationTestPackageInfo(item.Name)).ToList();

    }

    internal static async Task<string> GetPendingVerificationTest(VerificationTestPackageInfo? testInfo)
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
}