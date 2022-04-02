namespace GT3e.Acc.Models.Config.SeasonEntity;

public class SessionOnline
{
    public int EventIndex { get; set; }
    public int SessionIndex { get; set; }
    public int SingleEvents { get; set; }
    public int LoopSeason { get; set; }
    public int MaxPing { get; set; }
    public int LagSecs { get; set; }
    public int LagCount { get; set; }
    public int SimulateAsOneManTeam { get; set; }
    public int ForcePlayerSwap { get; set; }
    public int CockpitCameraMode { get; set; }
    public int DashCameraMode { get; set; }
    public int ChaseCameraMode { get; set; }
    public int BumperCameraMode { get; set; }
    public int BonnetCameraMode { get; set; }
    public int FreeJoinMode { get; set; }
}