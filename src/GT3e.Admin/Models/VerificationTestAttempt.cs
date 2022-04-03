using System;

namespace GT3e.Admin.Models;

public class VerificationTestAttempt
{
    public string AverageLapTime { get; set; }
    public string FastestLapTime { get; set; }
    public int FinishPosition { get; set; }
    public int InvalidLaps { get; set; }
    public bool Rejected { get; set; }
    public string RejectionReason { get; set; }
    public DateTime ReviewDate { get; set; }
    public int TotalLaps { get; set; }
}