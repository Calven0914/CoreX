using Sandbox;
using System;
using System.Linq;

public class NetworkManager : Component, Component.INetworkListener
{
    [Property] public GameObject PlayerPrefab { get; set; }

    public void OnConnected( Connection conn )
    {
        var steamId = conn.SteamId.ToString();

        if ( CoreXAdminPlugin.Bans != null && CoreXAdminPlugin.Bans.IsBanned( steamId ) )
        {
            var ban = CoreXAdminPlugin.Bans.GetBan( steamId );
            conn.Kick( $"You are banned: {ban.Reason} — {ban.TimeRemaining} remaining" );
            Log.Info( $"CoreX: kicked banned player {conn.DisplayName}" );
        }
    }

        public void OnActive( Connection conn )
    {
        Log.Info( $"CoreX: OnActive fired for {conn.DisplayName}, IsHost: {conn.IsHost}" );

        if ( PlayerPrefab == null )
        {
            Log.Warning( "CoreX: PlayerPrefab not set!" );
            return;
        }

        // destroy all existing players for this connection first
        var existing = Game.ActiveScene.GetAllObjects( true )
            .Where( o => o.Network.OwnerId == conn.Id )
            .ToList();

        foreach ( var obj in existing )
        {
            Log.Info( $"CoreX: destroying existing player for {conn.DisplayName}" );
            obj.Destroy();
        }

        // also destroy any unowned players
        var unowned = Game.ActiveScene.GetAllObjects( true )
            .Where( o => o.Network.OwnerId == Guid.Empty && o.Name == "PlayerPrefab" )
            .ToList();

        foreach ( var obj in unowned )
        {
            Log.Info( $"CoreX: destroying unowned player" );
            obj.Destroy();
        }

        var player = PlayerPrefab.Clone( Vector3.Zero );
        player.Name = conn.DisplayName;
        player.NetworkSpawn( conn );

        Log.Info( $"CoreX: spawned player for {conn.DisplayName}" );
    }
}