namespace GT3e.Acc.Messages;

public class RealtimeUpdate
{
    public string ActiveCamera { get; internal set; } = null!;
    public string ActiveCameraSet { get; internal set; } = null!;
    public byte AmbientTemp { get; internal set; }
    public ushort BestLapCarIndex { get; internal set; }
    public ushort BestLapDriverIndex { get; internal set; }
    public LapInfo BestSessionLap { get; internal set; } = null!;
    public float Clouds { get; internal set; }
    public string CurrentHudPage { get; internal set; } = null!;
    public int EventIndex { get; internal set; }
    public int FocusedCarIndex { get; internal set; }
    public bool IsReplayPlaying { get; internal set; }
    public SessionPhase Phase { get; internal set; }
    public float RainLevel { get; internal set; }
    public TimeSpan RemainingTime { get; internal set; }
    public float ReplayRemainingTime { get; internal set; }
    public float ReplaySessionTime { get; internal set; }
    public TimeSpan SessionEndTime { get; internal set; }
    public int SessionIndex { get; internal set; }
    public TimeSpan SessionRemainingTime { get; internal set; }
    public TimeSpan SessionTime { get; internal set; }
    public RaceSessionType SessionType { get; internal set; }
    public TimeSpan TimeOfDay { get; internal set; }
    public byte TrackTemp { get; internal set; }
    public float Wetness { get; internal set; }

    public override string ToString()
    {
        return
            $"Real Time Update: Session Type: {this.SessionType} Phase: {this.Phase}, Remaining Minutes: {this.SessionRemainingTime}";
    }
}