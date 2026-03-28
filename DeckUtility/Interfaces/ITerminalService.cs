using System;
using System.Threading.Tasks;

namespace DeckUtility.Interfaces;

public enum TerminalStatus
{
    Idle,
    Running,
    Success,
    Error
}

public interface ITerminalService
{
    string Name { get; }
    TerminalStatus Status { get; }
    string Logs { get; }
    event Action<string>? LogReceived;
    event Action<TerminalStatus>? StatusChanged;
    
    Task<int> ExecuteCommandAsync(string command, string? workingDirectory = null);
    void ClearLogs();
}

