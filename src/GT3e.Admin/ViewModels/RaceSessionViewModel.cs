using GT3e.Acc.Models.RaceResult;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GT3e.Admin.ViewModels;

public class RaceSessionViewModel : ObservableObject
{
    private string averageLapTime;
    private string driverFirstName;
    private string driverLastName;
    private string fastestLapTime;

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

    private void MapRaceSession(RaceSession raceSession)
    {
        var player = raceSession.GetPlayer();
        this.DriverFirstName = player.FirstName;
        this.DriverLastName = player.LastName;

        var playerLeaderBoardLine = raceSession.GetPlayerLeaderBoardLine();
        this.FastestLapTime = playerLeaderBoardLine.Timing.BestLapTime;
        this.AverageLapTime = playerLeaderBoardLine.Timing.AverageLapTime;

    }
}