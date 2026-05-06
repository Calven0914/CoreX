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

        var playerObj = Game.ActiveScene.GetAllObjects( true )
            .FirstOrDefault( o => o.Network.OwnerId == conn.Id && o.Name == conn.DisplayName );

        if ( playerObj == null )
        {
            Log.Info( $"CoreX: could not find player object for {conn.DisplayName}" );
            return;
        }

        var jailPosition = playerObj.WorldPosition;
        CoreXAdminPlugin.Jail.JailAtPosition( conn.SteamId.ToString(), jailPosition );

        // tell the client to jail themselves
        var enforcer = playerObj.Components.Get<JailEnforcer>( FindMode.EverythingInSelfAndDescendants );
        if ( enforcer != null )
        {
            enforcer.SetJailed( true, jailPosition );
            Log.Info( $"CoreX: sent jail RPC to {conn.DisplayName}" );
        }
        else
        {
            Log.Warning( $"CoreX: JailEnforcer not found on {conn.DisplayName}'s player object" );
        }

        CoreXAdminPlugin.Logs.Write( "Admin", "unknown", "jail", conn.DisplayName, reason );
        Log.Info( $"CoreX: jailed {conn.DisplayName} at {jailPosition}" );
    }
}