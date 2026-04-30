using Sandbox;
using System.Collections.Generic;

public class PermissionService
{
    private Dictionary<string, string> _steamIdToGroup = new()
    {
        { "76561198185107671", "superadmin" }  // replace with your Steam ID
    };

    private Dictionary<string, List<string>> _groupPermissions = new()
    {
        { "superadmin", new List<string> { "*" } },
        { "admin", new List<string> { "corex.kick", "corex.mute", "corex.teleport" } },
        { "user", new List<string>() }
    };

    public string GetGroup( string steamId )
    {
        return _steamIdToGroup.GetValueOrDefault( steamId, "user" );
    }

    public bool HasPermission( string steamId, string permission )
    {
        var group = GetGroup( steamId );
        var perms = _groupPermissions.GetValueOrDefault( group, new List<string>() );

        if ( perms.Contains( "*" ) ) return true;

        return perms.Contains( permission );
    }
}