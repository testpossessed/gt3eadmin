namespace GT3e.Acc.Models.Config.SeasonEntity;

public class SessionRealism
{
    public int PresetIndex { get; set; }
    public string Description { get; set; }
    public int EventIndex { get; set; }
    public int SessionIndex { get; set; }
    public double DamageRate { get; set; }
    public int Penalties { get; set; }
    public int TyreFuelWear { get; set; }
    public int UnlimitedTyreSet { get; set; }
    public int BrakeFading { get; set; }
    public int FastRollingStart { get; set; }
    public int IsTeleportAllowed { get; set; }
}