namespace GT3e.Acc.Models.Config.SeasonEntity;

public class Event
{
    public string TrackName { get; set; } = null!;
    public Circuit Circuit { get; set; } = null!;
    public Graphics Graphics { get; set; } = null!;
    public CarSet CarSet { get; set; } = null!;
    public Race Race { get; set; } = null!;
    public StartupWeatherStatus StartupWeatherStatus { get; set; } = null!;
    public StartupWeatherData StartupWeatherData { get; set; } = null!;
}