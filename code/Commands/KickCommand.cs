using Sandbox;
using System.Linq;

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

        // find the connection by name
        var conn = Connection.All
            .FirstOrDefault( c => c.DisplayName.ToLower().Contains( target.ToLower() ) );

        if ( conn == null )
        {
            Log.Info( $"CoreX: player '{target}' not found" );
            return;
        }

        CoreXAdminPlugin.Logs.Write(
            "Admin",
            "unknown",
            "kick",
            conn.DisplayName,
            reason
        );

        Log.Info( $"CoreX: kicking {conn.DisplayName}" );
        conn.Kick( reason );
    }
}