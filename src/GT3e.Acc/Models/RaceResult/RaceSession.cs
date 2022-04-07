namespace GT3e.Acc.Models.RaceResult;

public class RaceSession
{
    public int InvalidLaps
    {
        get
        {
            var carId = this.GetPlayerLeaderBoardLine()
                            .Car.CarId;
            return this.Laps.Count(l => l.CarId == carId && l.Flags != 0);
        }
    }

    public int TotalLaps =>
        this.GetPlayerLeaderBoardLine()
            .Timing.LapCount;
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

    public int GetPlayerFinishPosition()
    {
        var playerLeaderBoardLine = this.GetPlayerLeaderBoardLine();
        return this.SnapShot.LeaderBoardLines.IndexOf(playerLeaderBoardLine) + 1;
    }
}