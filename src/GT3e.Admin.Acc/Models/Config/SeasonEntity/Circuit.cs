namespace GT3e.Acc.Models.Config.SeasonEntity;

public class Circuit
{
    public bool Enabled { get; set; }
    public int PitNumber { get; set; }
    public int SectorCount { get; set; }
    public double FormationTriggerNormalizedRangeStart { get; set; }
    public double GreenFlagTriggerNormalizedRangeStart { get; set; }
    public double GreenFlagTriggerNormalizedRangeEnd { get; set; }
    public double BaseGrip { get; set; }
}