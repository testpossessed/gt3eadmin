namespace GT3e.Acc.Models.Config.SeasonEntity;

public class StartupWeatherData
{
    public int IsDynamic { get; set; }
    public double AmbientTemperatureMean { get; set; }
    public double WindSpeed { get; set; }
    public double WindSpeedMean { get; set; }
    public double WindSpeedDeviation { get; set; }
    public double WindDirection { get; set; }
    public double WindDirectionChange { get; set; }
    public int WindHarmonic { get; set; }
    public int NHarmonics { get; set; }
    public double WeatherBaseMean { get; set; }
    public double WeatherBaseDeviation { get; set; }
    public double VariabilityDeviation { get; set; }
    public List<object> SineCoefficients { get; set; }
    public List<object> CosineCoefficients { get; set; }
}