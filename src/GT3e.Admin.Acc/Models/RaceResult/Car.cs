namespace GT3e.Acc.Models.RaceResult;

public class Car
{
    public int CarId { get; set; }
    public int RaceNumber { get; set; }
    public int CarModel { get; set; }
    public int CupCategory { get; set; }
    public string CarGroup { get; set; }
    public string TeamName { get; set; }
    public int Nationality { get; set; }
    public int CarGuid { get; set; }
    public int TeamGuid { get; set; }
    public List<Driver> Drivers { get; set; }
}