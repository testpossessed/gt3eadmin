namespace GT3e.Acc.Models.RaceResult;

public class LeaderBoardLine
{
    public Car Car { get; set; } = null!;
    public CurrentDriver CurrentDriver { get; set; } = null!;
    public int CurrentDriverIndex { get; set; }
    public Timing Timing { get; set; } = null!;
    public int MissingMandatoryPitstop { get; set; }
    public List<double> DriverTotalTimes { get; set; } = null!;
}