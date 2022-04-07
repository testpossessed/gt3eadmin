namespace GT3e.Acc.Messages;

public class ConnectionState
{
    public ConnectionState(int connectionId, bool isConnected, bool isReadOnly, string? error = null)
    {
        this.ConnectionId = connectionId;
        this.IsConnected = isConnected;
        this.IsReadOnly = isReadOnly;
        this.Error = error;
    }

    public int ConnectionId { get; }
    public string? Error { get; }
    public bool IsConnected { get; }
    public bool IsReadOnly { get; }

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(this.Error)
            ? $"Connection State: ID: {this.ConnectionId} Connected: {this.IsConnected} Read Only: {this.IsReadOnly}"
            : $"Connection State: ID: {this.ConnectionId} ERROR: {this.Error}";
    }
}