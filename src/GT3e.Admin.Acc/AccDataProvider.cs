using GT3e.Acc.Models.RaceResult;
using Newtonsoft.Json;

namespace GT3e.Acc;

public class AccDataProvider
{
    public static RaceSession? LoadRaceSession(string filePath)
    {
        var json = CleanJson(File.ReadAllText(filePath));
        return JsonConvert.DeserializeObject<RaceSession>(json);
    }

    private static string CleanJson(string json)
    {
        return json.Replace("\0", "").Replace("\n", "");
    }
}