namespace GT3e.Acc.Models.RaceResult;

public class Car
{
    public int CarId { get; set; }
    public int RaceNumber { get; set; }
    public int CarModel { get; set; }
    public int CupCategory { get; set; }
    public string CarGroup { get; set; } = null!;
    public string TeamName { get; set; } = null!;
    public int Nationality { get; set; }
    public int CarGuid { get; set; }
    public int TeamGuid { get; set; }
    public List<Driver> Drivers { get; set; } = null!;
}