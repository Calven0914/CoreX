using Sandbox;

public class KickCommand : BaseCommand
{
    public override string Name => "kick";
    public override string[] Aliases => new string[] { "k" };
    public override string RequiredPermission => "corex.kick";
    public override string Usage => "!kick <playername>";

    public override void Execute( GameObject caller, string[] args )
    {
        if ( args.Length == 0 )
        {
            Log.Info( "Usage: " + Usage );
            return;
        }

        var target = args[0];
        var reason = args.Length > 1 ? string.Join( " ", args[1..] ) : "Kicked by admin";

        CoreXAdminPlugin.Logs.Write(
            "Admin",
            "unknown",
            "kick",
            target,
            reason
        );

        Log.Info( $"CoreX: would kick '{target}' for '{reason}'" );
    }
}