using System.Collections.Generic;

namespace GT3e.Admin.Models;

public class DriverStats
{
    public DriverStats()
    {
        this.VerificationTestAttempts = new List<VerificationTestAttempt>();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SteamId { get; set; }
    public List<VerificationTestAttempt> VerificationTestAttempts { get; set; }

    public void AddVerificationTestAttempt(VerificationTestAttempt verificationTestAttempt)
    {
        this.VerificationTestAttempts ??= new List<VerificationTestAttempt>();

        this.VerificationTestAttempts.Add(verificationTestAttempt);
    }
}