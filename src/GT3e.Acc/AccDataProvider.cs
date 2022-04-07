using System.Runtime.InteropServices.ComTypes;
using GT3e.Acc.Models.Customs;
using GT3e.Acc.Models.RaceResult;
using Newtonsoft.Json;

namespace GT3e.Acc;

public class AccDataProvider
{
    private static IList<string> validCarCodes = new List<string>
    {
        "AMRV8",
        "AR8EVO",
        "AR8EVOII",
        "BENTC",
        "BMWM4",
        "BMWM6",
        "FER488",
        "FER488EVO",
        "HONNSX",
        "LAMHUR",
        "LAMHUREVO",
        "LEXUSRC",
        "MC720S",
        "MERCAMG",
        "MERCAMGEVO",
        "NISGTR",
        "PO991II"
    };

    public static IEnumerable<CustomCar> GetCustomCars()
    {
        var filePaths = Directory.GetFiles(AccPathProvider.CustomCarsFolderPath, "*.json");

        return filePaths.Select(filePath =>
                                    JsonConvert.DeserializeObject<CustomCar>(
                                        CleanJson(File.ReadAllText(filePath))))
                        .ToList();
    }

    public static IEnumerable<CustomSkin> GetCustomSkins()
    {
        var folderPaths = Directory.GetDirectories(AccPathProvider.CustomLiveriesFolderPath);

        var result = new List<CustomSkin>();

        foreach(var folderPath in folderPaths)
        {
            var folderName = Path.GetRelativePath(AccPathProvider.CustomLiveriesFolderPath, folderPath);
            if(MatchesNamingConvention(folderName))
            {
                result.Add(new CustomSkin
                {
                    Name = folderName,
                    FolderPath = folderPath
                });
            }
        }

        return result;
    }

    private static bool MatchesNamingConvention(string folderName)
    {
        var elements = folderName.Split("-", StringSplitOptions.RemoveEmptyEntries);
        if(elements.Length != 4)
        {
            return false;
        }

        if(!int.TryParse(elements[0], out var raceNumber))
        {
            return false;
        }

        if(!validCarCodes.Contains(elements[2]))
        {
            return false;
        }

        return elements[3] == "GT3";
    }

    public static RaceSession? LoadRaceSession(string filePath)
    {
        Thread.Sleep(1000);
        try
        {
            var fileContent = File.ReadAllText(filePath);
            var json = CleanJson(fileContent);
            return JsonConvert.DeserializeObject<RaceSession>(json);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string CleanJson(string json)
    {
        return json.Replace("\0", "")
                   .Replace("\n", "");
    }
}