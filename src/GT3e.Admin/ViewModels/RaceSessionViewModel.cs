using GT3e.Acc.Models.RaceResult;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GT3e.Admin.ViewModels;

public class RaceSessionViewModel : ObservableObject
{
    private string averageLapTime;
    private string driverFirstName;
    private string driverLastName;
    private string fastestLapTime;
    private int finishPosition;
    private int invalidLaps;
    private int totalLaps;

    public RaceSessionViewModel(RaceSession raceSession)
    {
        this.MapRaceSession(raceSession);
    }

    public string AverageLapTime
    {
        get => this.averageLapTime;
        set => this.SetProperty(ref this.averageLapTime, value);
    }

    public string DriverFirstName
    {
        get => this.driverFirstName;
        set => this.SetProperty(ref this.driverFirstName, value);
    }

    public string DriverLastName
    {
        get => this.driverLastName;
        set => this.SetProperty(ref this.driverLastName, value);
    }

    public string FastestLapTime
    {
        get => this.fastestLapTime;
        set => this.SetProperty(ref this.fastestLapTime, value);
    }

    public int FinishPosition
    {
        get => this.finishPosition;
        set => this.SetProperty(ref this.finishPosition, value);
    }

    public int InvalidLaps { get => this.invalidLaps; set => this.SetProperty(ref this.invalidLaps, value); }

    public int TotalLaps { get => this.totalLaps; set => this.SetProperty(ref this.totalLaps, value); }

    private void MapRaceSession(RaceSession raceSession)
    {
        var player = raceSession.GetPlayer();
        this.DriverFirstName = player.FirstName;
        this.DriverLastName = player.LastName;

        var playerLeaderBoardLine = raceSession.GetPlayerLeaderBoardLine();
        this.FastestLapTime = playerLeaderBoardLine.Timing.BestLapTime;
        this.AverageLapTime = playerLeaderBoardLine.Timing.AverageLapTime;

        this.TotalLaps = raceSession.TotalLaps;
        this.InvalidLaps = raceSession.InvalidLaps;
        this.FinishPosition = raceSession.GetPlayerFinishPosition();
    }
}