using Sandbox;
using System.Collections.Generic;

public class JailService
{
    private readonly Dictionary<string, Vector3> _jailed = new();

    public void JailAtPosition( string steamId, Vector3 position )
    {
        _jailed[steamId] = position;
        Log.Info( $"CoreX: jailed {steamId} at {position}" );
    }

    public void Unjail( string steamId )
    {
        _jailed.Remove( steamId );
    }

    public bool IsJailed( string steamId )
    {
        return _jailed.ContainsKey( steamId );
    }

    public Vector3 GetJailPosition( string steamId )
    {
        return _jailed.GetValueOrDefault( steamId, Vector3.Zero );
    }
}