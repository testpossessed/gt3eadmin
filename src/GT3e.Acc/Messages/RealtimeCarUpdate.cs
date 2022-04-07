namespace GT3e.Acc.Messages;

public class RealtimeCarUpdate
{
    public int CarIndex { get; internal set; }
    public int DriverIndex { get; internal set; }
    public int Gear { get; internal set; }
    public float WorldPosX { get; internal set; }
    public float WorldPosY { get; internal set; }
    public float Yaw { get; internal set; }
    public CarLocation CarLocation { get; internal set; }
    public int Kmh { get; internal set; }
    public int Position { get; internal set; }
    public int TrackPosition { get; internal set; }
    public float SplinePosition { get; internal set; }
    public int Delta { get; internal set; }
    public LapInfo BestSessionLap { get; internal set; } = null!;
    public LapInfo LastLap { get; internal set; } = null!;
    public LapInfo CurrentLap { get; internal set; } = null!;
    public int Laps { get; internal set; }
    public ushort CupPosition { get; internal set; }
    public byte DriverCount { get; internal set; }

    public override string ToString()
    {
        return $"Real Time Care Update: Car Index: {this.CarIndex}, Driver Index: {this.DriverIndex}, Lap Info: {this.CurrentLap}";
    }
}