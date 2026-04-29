using Sandbox;
using System.Collections.Generic;

public class LogEntry
{
    public string Timestamp { get; set; }
    public string ExecutorName { get; set; }
    public string ExecutorSteamId { get; set; }
    public string Action { get; set; }
    public string Target { get; set; }
    public string Reason { get; set; }
}

public class LogService
{
    private readonly List<LogEntry> _logs = new();

    public void Write( string executorName, string executorSteamId, string action, string target, string reason = "No reason given" )
    {
        var entry = new LogEntry
        {
            Timestamp = System.DateTime.UtcNow.ToString( "yyyy-MM-dd HH:mm:ss" ),
            ExecutorName = executorName,
            ExecutorSteamId = executorSteamId,
            Action = action,
            Target = target,
            Reason = reason
        };

        _logs.Add( entry );
        Log.Info( $"[CoreX Log] {entry.Timestamp} | {executorName} | {action} | {target} | {reason}" );
    }

    public IReadOnlyList<LogEntry> GetAll() => _logs;

    public IReadOnlyList<LogEntry> GetLast( int count )
    {
        var start = System.Math.Max( 0, _logs.Count - count );
        return _logs.GetRange( start, _logs.Count - start );
    }
}