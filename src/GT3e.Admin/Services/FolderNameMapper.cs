namespace GT3e.Admin.Services
{
  internal static class FolderNameMapper
  {
    internal static string GetFriendlyCircuitName(string folderName)
    {
      var trackFolderConfig = Configuration.GetSection("TrackFolders");
      return trackFolderConfig[folderName];
    }

    internal static string GetFriendlyVehicleName(string folderName)
    {
      var carFolderConfig = Configuration.GetSection("CarFolders");
      return carFolderConfig[folderName];
    }
  }
}