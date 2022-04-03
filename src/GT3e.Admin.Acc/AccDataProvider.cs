using GT3e.Acc.Models.RaceResult;
using Newtonsoft.Json;

namespace GT3e.Acc;

public class AccDataProvider
{
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
        return json.Replace("\0", "").Replace("\n", "");
    }
}