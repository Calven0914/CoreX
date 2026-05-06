using Sandbox;

public class JailEnforcer : Component
{
    public bool IsJailed { get; set; } = false;
    public Vector3 JailPosition { get; set; }

    protected override void OnUpdate()
    {
        if ( !IsJailed ) return;
        if ( IsProxy ) return; // only run on the owner's client

        // force position on the client side
        WorldPosition = JailPosition;

        var pc = Components.Get<PlayerController>( FindMode.EverythingInSelfAndDescendants );
        if ( pc != null ) pc.Enabled = false;

        var rb = Components.Get<Rigidbody>( FindMode.EverythingInSelfAndDescendants );
        if ( rb != null )
        {
            rb.Velocity = Vector3.Zero;
            rb.AngularVelocity = Vector3.Zero;
        }
    }

    [Rpc.Owner]
    public void SetJailed( bool jailed, Vector3 position )
    {
        IsJailed = jailed;
        JailPosition = position;

        var pc = Components.Get<PlayerController>( FindMode.EverythingInSelfAndDescendants );
        if ( pc != null ) pc.Enabled = !jailed;

        Log.Info( $"CoreX: jail state set to {jailed} at {position}" );
    }
}