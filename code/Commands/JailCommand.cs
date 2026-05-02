using Sandbox;
using System.Linq;

public class JailCommand : BaseCommand
{
    public override string Name => "jail";
    public override string RequiredPermission => "corex.jail";
    public override string Usage => "!jail <playername> [reason]";

    public override void Execute( GameObject caller, string[] args )
    {
        if ( args.Length == 0 )
        {
            Log.Info( "Usage: " + Usage );
            return;
        }

        var target = args[0];
        var reason = args.Length > 1 ? string.Join( " ", args[1..] ) : "Jailed by admin";

        var conn = Connection.All
            .FirstOrDefault( c => c.DisplayName.ToLower().Contains( target.ToLower() ) );

        if ( conn == null )
        {
            Log.Info( $"CoreX: player '{target}' not found" );
            return;
        }

        if ( conn.IsHost )
        {
            Log.Info( "CoreX: cannot jail the host" );
            return;
        }

        // find player object
        var playerObj = Game.ActiveScene.GetAllObjects( true )
            .FirstOrDefault( o => o.Network.OwnerId == conn.Id );

        if ( playerObj == null )
        {
            Log.Info( $"CoreX: could not find player object for {conn.DisplayName}" );
            return;
        }

        // jail them on the spot where they currently are
        var jailPosition = playerObj.WorldPosition;
        CoreXAdminPlugin.Jail.JailAtPosition( conn.SteamId.ToString(), jailPosition );

        CoreXAdminPlugin.Logs.Write(
            "Admin", "unknown", "jail", conn.DisplayName, reason
        );

        Log.Info( $"CoreX: jailed {conn.DisplayName} at {jailPosition}" );
    }
}