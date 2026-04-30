using System;

public class BanRecord
{
    public string SteamId { get; set; }
    public string DisplayName { get; set; }
    public string Reason { get; set; }
    public string BannedBy { get; set; }
    public DateTime BannedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsPermanent { get; set; }

    public bool IsExpired => !IsPermanent && DateTime.UtcNow > ExpiresAt;

    public string TimeRemaining
    {
        get
        {
            if ( IsPermanent ) return "Permanent";
            var remaining = ExpiresAt - DateTime.UtcNow;
            if ( remaining.TotalMinutes < 1 ) return "Expired";
            if ( remaining.TotalHours < 1 ) return $"{(int)remaining.TotalMinutes}min";
            if ( remaining.TotalDays < 1 ) return $"{(int)remaining.TotalHours}h {remaining.Minutes}min";
            return $"{(int)remaining.TotalDays}d {remaining.Hours}h";
        }
    }
}