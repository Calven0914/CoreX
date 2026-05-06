using Sandbox;
using System.Linq;

public class CoreXAdminPlugin : Component
{
    public static PermissionService Permissions { get; private set; }
    public static CommandService Commands { get; private set; }
    public static LogService Logs { get; private set; }
    public static BanService Bans { get; private set; }
    public static JailService Jail { get; private set; }

    protected override void OnStart()
    {
        Permissions = new PermissionService();
        Logs = new LogService();
        Bans = new BanService();
        Jail = new JailService();
        Commands = new CommandService( Permissions );

        Commands.Register( new KickCommand() );
        Commands.Register( new BanCommand() );
        Commands.Register( new JailCommand() );
        Commands.Register( new UnjailCommand() );

        Log.Info( "CoreX Admin: initialized." );
    }

    protected override void OnUpdate()
    {
        
    }

    [Rpc.Host]
    public static void SendChatCommand( string message )
    {
        var caller = Rpc.Caller;
        var steamId = caller.SteamId.ToString();
        Commands.Handle( null, steamId, message );
    }
}