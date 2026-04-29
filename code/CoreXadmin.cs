using Sandbox;

public class CoreXAdminPlugin : Component
{
    public static PermissionService Permissions { get; private set; }

    protected override void OnStart()
    {
        Permissions = new PermissionService();
        // no Load() needed anymore

        Log.Info( "CoreX Admin: initialized." );

        // temporary test
        var testId = "76561198000000000"; // your steam ID
        Log.Info( "Group: " + Permissions.GetGroup( testId ) );
        Log.Info( "Can kick: " + Permissions.HasPermission( testId, "corex.kick" ) );
    }
}