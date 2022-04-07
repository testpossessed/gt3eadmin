namespace GT3e.Acc.Models.RaceResult;

public class Timing
{
    public int LastLap { get; set; }
    public List<int> LastSplits { get; set; } = null!;
    public int BestLap { get; set; }
    public List<int> BestSplits { get; set; } = null!;
    public int TotalTime { get; set; }
    public int LapCount { get; set; }
    public int LastSplitId { get; set; }

    public string BestLapTime =>
        TimeSpan.FromMilliseconds(this.BestLap)
                .ToString("mm\\:ss\\.fff");
    public string AverageLapTime =>
        TimeSpan.FromMilliseconds((double) this.TotalTime / this.LapCount)
                .ToString("mm\\:ss\\.fff");
}