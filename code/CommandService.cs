using Sandbox;
using System.Collections.Generic;

public class CommandService
{
    private readonly Dictionary<string, BaseCommand> _commands = new();
    private readonly PermissionService _permissions;
    public int CommandCount => _commands.Count;
    public CommandService( PermissionService permissions )
    {
        _permissions = permissions;
    }

    public void Register( BaseCommand command )
    {
        _commands[command.Name] = command;
        foreach ( var alias in command.Aliases )
            _commands[alias] = command;

        Log.Info( $"CoreX Admin: registered command '{command.Name}'" );
    }

    public void Handle( GameObject caller, string steamId, string input )
    {
        if ( !input.StartsWith( "!" ) && !input.StartsWith( "/" ) ) return;

        var parts = input.TrimStart( '!', '/' ).Split( ' ' );
        var commandName = parts[0].ToLower();
        var args = parts[1..];

        if ( !_commands.TryGetValue( commandName, out var command ) )
        {
            Log.Info( $"CoreX: unknown command '{commandName}'" );
            return;
        }

        if ( !_permissions.HasPermission( steamId, command.RequiredPermission ) )
        {
            Log.Info( $"CoreX: permission denied for '{commandName}'" );
            return;
        }

        Log.Info( $"CoreX: executing '{commandName}'" );
        command.Execute( caller, args );
    }
}