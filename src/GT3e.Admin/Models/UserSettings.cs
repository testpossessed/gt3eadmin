namespace GT3e.Admin.Models
{
  internal class UserSettings
  {
    public string Theme { get; set; } = null!;
    public bool IsInitialised { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public bool IsVerificationPending { get; set; } = false;
    public string SteamId { get; set; } = null!;
  }
}