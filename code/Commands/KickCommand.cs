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

        Log.Info( $"CoreX: kick command received for target '{args[0]}'" );
        // actual kick logic will go here once we have player management set up
    }
}