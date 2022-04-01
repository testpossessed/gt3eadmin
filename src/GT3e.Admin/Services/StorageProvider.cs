using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Blobs;
using GT3e.Admin.Models;

namespace GT3e.Admin.Services;

internal class StorageProvider
{
    internal static IEnumerable<VerificationTestPackageInfo> GetPendingVerificationTests()
    {
        var systemSettings = SettingsProvider.GetSystemSettings();

        var blobContainerClient =
            new BlobContainerClient(systemSettings.StorageConnectionString, "verification-tests");
        var containerUrl = blobContainerClient.Uri.ToString();
        var pages = blobContainerClient.GetBlobs().AsPages();

        return (from page in pages from item in page.Values select new VerificationTestPackageInfo(item.Name, $"{containerUrl}")).ToList();

    }
}