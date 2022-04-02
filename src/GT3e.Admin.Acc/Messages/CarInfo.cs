namespace GT3e.Acc.Messages;

public class CarInfo
{
    public CarInfo(ushort carIndex)
    {
        this.CarIndex = carIndex;
    }

    public ushort CarIndex { get; }
    public IList<DriverInfo> Drivers { get; } = new List<DriverInfo>();
    public byte CarModelType { get; internal set; }
    public byte CupCategory { get; internal set; }
    public int CurrentDriverIndex { get; internal set; }
    public Nationality Nationality { get; internal set; }
    public int RaceNumber { get; internal set; }
    public string TeamName { get; internal set; }

    public string GetCurrentDriverName()
    {
        return this.CurrentDriverIndex < this.Drivers.Count
                   ? this.Drivers[this.CurrentDriverIndex]
                         .LastName
                   : "nobody(?)";
    }

    internal void AddDriver(DriverInfo driverInfo)
    {
        this.Drivers.Add(driverInfo);
    }

    public override string ToString()
    {
        return $"RaceSession Number: {this.RaceNumber}, Driver: {this.Drivers[this.CurrentDriverIndex].ShortName} , Team: {this.TeamName}";
    }
}