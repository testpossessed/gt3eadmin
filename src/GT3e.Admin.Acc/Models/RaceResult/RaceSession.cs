namespace GT3e.Acc.Models.RaceResult;

public class RaceSession
{
    public bool HasBeenSkipped { get; set; }
    public List<Lap> Laps { get; set; } = null!;
    public SessionDef SessionDef { get; set; } = null!;
    public SnapShot SnapShot { get; set; } = null!;

    public CurrentDriver GetPlayer()
    {
        return this.SnapShot.GetPlayerLeaderBoardLine()
                   .CurrentDriver;
    }

    public LeaderBoardLine GetPlayerLeaderBoardLine()
    {
        return this.SnapShot.GetPlayerLeaderBoardLine();
    }
}