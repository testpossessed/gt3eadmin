namespace GT3e.Acc.Models.Config.SeasonEntity;

public class Assists
{
    public int AllowAutoSteer { get; set; }
    public int AutoClutch { get; set; }
    public int AutoEngineSwitch { get; set; }
    public int AutoGear { get; set; }
    public int AutoLights { get; set; }
    public int AutoPitLimiter { get; set; }
    public int AutoWiper { get; set; }
    public string Description { get; set; } = null!;
    public int PresetIndex { get; set; }
    public int ShowIdealLine { get; set; }
    public double StabilityControlLevelMax { get; set; }
    public double StabilityControlLevelMin { get; set; }
}