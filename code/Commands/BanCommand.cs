using Sandbox;
using System.Linq;

public class BanCommand : BaseCommand
{
    public override string Name => "ban";
    public override string[] Aliases => new string[] { "b" };
    public override string RequiredPermission => "corex.ban";
    public override string Usage => "!ban <playername> <minutes> [reason]";

    public override void Execute( GameObject caller, string[] args )
    {
        if ( args.Length == 0 )
        {
            Log.Info( "Usage: " + Usage );
            return;
        }

        var target = args[0];
        var duration = 0; // 0 = permanent

        // try to parse duration from second arg
        if ( args.Length > 1 && int.TryParse( args[1], out var mins ) )
        {
            duration = mins;
        }

        // reason is everything after name and duration
        var reasonStart = args.Length > 1 && int.TryParse( args[1], out _ ) ? 2 : 1;
        var reason = args.Length > reasonStart
            ? string.Join( " ", args[reasonStart..] )
            : "Banned by admin";

        var conn = Connection.All
            .FirstOrDefault( c => c.DisplayName.ToLower().Contains( target.ToLower() ) );

        if ( conn == null )
        {
            Log.Info( $"CoreX: player '{target}' not found" );
            return;
        }

        if ( conn.IsHost )
        {
            Log.Info( "CoreX: cannot ban the host" );
            return;
        }

        // record the ban
        CoreXAdminPlugin.Bans.Ban(
            conn.SteamId.ToString(),
            conn.DisplayName,
            reason,
            "Admin",
            duration
        );

        CoreXAdminPlugin.Logs.Write(
            "Admin",
            "unknown",
            "ban",
            conn.DisplayName,
            reason
        );

        // kick them with the ban reason
        var durationText = duration > 0 ? $"{duration} minutes" : "permanently";
        conn.Kick( $"Banned {durationText}: {reason}" );
    }
}