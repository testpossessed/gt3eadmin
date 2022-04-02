namespace GT3e.Acc.Models.RaceResult;

public class SnapShot
{
    public int Bestlap { get; set; }
    public List<int> BestSplits { get; set; } = null!;
    public int IsWetSession { get; set; }
    public int Type { get; set; }
    public List<LeaderBoardLine> LeaderBoardLines { get; set; } = null!;

    internal LeaderBoardLine GetPlayerLeaderBoardLine()
    {
        return this.LeaderBoardLines.First(e => e.CurrentDriver.PlayerId != "0");
    }
}