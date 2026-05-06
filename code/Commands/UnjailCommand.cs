using Sandbox;
using System.Linq;

public class UnjailCommand : BaseCommand
{
    public override string Name => "unjail";
    public override string RequiredPermission => "corex.jail";
    public override string Usage => "!unjail <playername>";

    public override void Execute( GameObject caller, string[] args )
    {
        if ( args.Length == 0 )
        {
            Log.Info( "Usage: " + Usage );
            return;
        }

        var target = args[0];

        var conn = Connection.All
            .FirstOrDefault( c => c.DisplayName.ToLower().Contains( target.ToLower() ) );

        if ( conn == null )
        {
            Log.Info( $"CoreX: player '{target}' not found" );
            return;
        }

        CoreXAdminPlugin.Jail.Unjail( conn.SteamId.ToString() );

        var playerObj = Game.ActiveScene.GetAllObjects( true )
            .FirstOrDefault( o => o.Network.OwnerId == conn.Id && o.Name == conn.DisplayName );

        if ( playerObj != null )
        {
            var enforcer = playerObj.Components.Get<JailEnforcer>( FindMode.EverythingInSelfAndDescendants );
            if ( enforcer != null )
                enforcer.SetJailed( false, Vector3.Zero );
        }

        CoreXAdminPlugin.Logs.Write( "Admin", "unknown", "unjail", conn.DisplayName, "Released from jail" );
        Log.Info( $"CoreX: unjailed {conn.DisplayName}" );
    }
}