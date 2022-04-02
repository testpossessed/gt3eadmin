namespace GT3e.Acc.Messages;

public class EntryListUpdate
{
    public EntryListUpdate(string sender, CarInfo carInfo)
    {
        this.Sender = sender;
        this.CarInfo = carInfo;
    }

    public CarInfo CarInfo { get; }
    public string Sender { get; }

    public override string ToString()
    {
        return $"Entry List Update: Sender: {this.Sender}, Car Data: {this.CarInfo}";
    }
}