using Sandbox;
using CoreX.UI;

public class CoreXAdminPlugin : Component
{
    public static PermissionService Permissions { get; private set; }
    public static CommandService Commands { get; private set; }
    public static LogService Logs { get; private set; }

    protected override void OnStart()
    {
        Permissions = new PermissionService();
        Logs = new LogService();
        Commands = new CommandService( Permissions );

        Commands.Register( new KickCommand() );

        Log.Info( "CoreX Admin: initialized." );
    }

    [Rpc.Host]
    public static void SendChatCommand( string message )
    {
        var caller = Rpc.Caller;
        var steamId = caller.SteamId.ToString();

        Log.Info( $"CoreX: {caller.DisplayName} said: {message}" );
        Commands.Handle( null, steamId, message );
    }
}