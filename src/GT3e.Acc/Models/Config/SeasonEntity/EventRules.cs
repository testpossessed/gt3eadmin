namespace GT3e.Acc.Models.Config.SeasonEntity;

public class EventRules
{
    public int QualifyStandingType { get; set; }
    public int SuperpoleMaxCar { get; set; }
    public int PitWindowLengthSec { get; set; }
    public int DriverStintTimeSec { get; set; }
    public bool IsRefuellingAllowedInRace { get; set; }
    public bool IsRefuellingTimeFixed { get; set; }
    public int MaxDriversCount { get; set; }
    public int MandatoryPitstopCount { get; set; }
    public int MaxTotalDrivingTime { get; set; }
    public bool IsMandatoryPitstopRefuellingRequired { get; set; }
    public bool IsMandatoryPitstopTyreChangeRequired { get; set; }
    public bool IsMandatoryPitstopSwapDriverRequired { get; set; }
    public int TyreSetCount { get; set; }
}