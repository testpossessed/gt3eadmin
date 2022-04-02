namespace GT3e.Acc.Models.RaceResult;

public class Lap
{
    public int CarId { get; set; }
    public int DriverId { get; set; }
    public int LapTime { get; set; }
    public List<int> Splits { get; set; }
    public double Fuel { get; set; }
    public int Flags { get; set; }
    public double TimestampMS { get; set; }
}