namespace GT3e.Acc.Messages;

public struct DriverInfo
{
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string ShortName { get; internal set; }
    public DriverCategory Category { get; internal set; }
    public Nationality Nationality { get; internal set; }
}