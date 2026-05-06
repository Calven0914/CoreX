using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

public class BanService
{
    private readonly List<BanRecord> _bans = new();

    public void Ban( string steamId, string displayName, string reason, string bannedBy, int durationMinutes = 0 )
    {
        // remove existing ban if any
        _bans.RemoveAll( b => b.SteamId == steamId );

        var ban = new BanRecord
        {
            SteamId = steamId,
            DisplayName = displayName,
            Reason = reason,
            BannedBy = bannedBy,
            BannedAt = DateTime.UtcNow,
            IsPermanent = durationMinutes <= 0,
            ExpiresAt = durationMinutes > 0
                ? DateTime.UtcNow.AddMinutes( durationMinutes )
                : DateTime.MaxValue
        };

        _bans.Add( ban );
        Log.Info( $"CoreX: banned {displayName} ({steamId}) for '{reason}' — {ban.TimeRemaining}" );
    }

    public void Unban( string steamId )
    {
        var removed = _bans.RemoveAll( b => b.SteamId == steamId );
        Log.Info( removed > 0
            ? $"CoreX: unbanned {steamId}"
            : $"CoreX: no ban found for {steamId}" );
    }

    public bool IsBanned( string steamId )
    {
        var ban = _bans.FirstOrDefault( b => b.SteamId == steamId );
        if ( ban == null ) return false;
        
        Log.Info( $"CoreX: ban check for {steamId} — expires {ban.ExpiresAt} — now {DateTime.UtcNow} — expired: {ban.IsExpired}" );
        
        if ( ban.IsExpired )
        {
            _bans.Remove( ban );
            return false;
        }
        return true;
    }

    public BanRecord GetBan( string steamId )
    {
        return _bans.FirstOrDefault( b => b.SteamId == steamId && !b.IsExpired );
    }

    public IReadOnlyList<BanRecord> GetAllActive()
    {
        _bans.RemoveAll( b => b.IsExpired );
        return _bans;
    }
}