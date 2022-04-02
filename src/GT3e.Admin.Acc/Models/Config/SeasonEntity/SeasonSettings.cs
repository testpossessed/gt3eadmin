namespace GT3e.Acc.Models.Config.SeasonEntity;

public class SeasonSettings
{
    public Assists Assists { get; set; } = null!;
    public int EventOnlineReference { get; set; }
    public IList<Event> Events { get; set; } = new List<Event>();
    public Gameplay Gameplay { get; set; } = null!;
    public Graphics Graphics { get; set; } = null!;
    public SessionGameplay SessionGameplay { get; set; } = null!;
    public IList<SessionGameplay> SessionGameplayOverride { get; set; } = new List<SessionGameplay>();
    public SessionOnline SessionOnline { get; set; } = null!;
    public IList<SessionOnline> SessionOnlineOverride { get; set; } = new List<SessionOnline>();
    public SessionRealism SessionRealism { get; set; } = null!;
    public IList<SessionRealism> SessionRealismOverride { get; set; } = new List<SessionRealism>();
}