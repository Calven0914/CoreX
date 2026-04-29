using Sandbox;

public class CoreXAdminPlugin : Component
{
    public static PermissionService Permissions { get; private set; }
    public static CommandService Commands { get; private set; }

    protected override void OnStart()
    {
        Permissions = new PermissionService();
        Commands = new CommandService( Permissions );

        Commands.Register( new KickCommand() );

        Log.Info( "CoreX Admin: initialized." );
    }
}