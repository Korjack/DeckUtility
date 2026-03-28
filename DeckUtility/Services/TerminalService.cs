using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DeckUtility.Interfaces;

namespace DeckUtility.Services;

public class TerminalService : ITerminalService
{
    // Global static event to collect all logs from any instance
    public static event Action<string, string>? OnAnyLogReceived;

    private TerminalStatus _status = TerminalStatus.Idle;
    private readonly StringBuilder _logs = new();

    public string Name { get; }

    public TerminalStatus Status
    {
        get => _status;
        private set
        {
            if (_status != value)
            {
                _status = value;
                StatusChanged?.Invoke(_status);
            }
        }
    }

    public string Logs => _logs.ToString();

    public event Action<string>? LogReceived;
    public event Action<TerminalStatus>? StatusChanged;

    public TerminalService(string name)
    {
        Name = name;
    }

    public async Task<int> ExecuteCommandAsync(string command, string? workingDirectory = null)
    {
        if (Status == TerminalStatus.Running)
        {
            throw new InvalidOperationException($"[{Name}] A command is already running.");
        }

        Status = TerminalStatus.Running;
        AppendLog($"[INFO] [{Name}] Executing: {command}");

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory
        };

        using var process = new Process { StartInfo = processStartInfo };

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null) AppendLog(e.Data);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null) AppendLog($"[ERROR] {e.Data}");
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
            
            var exitCode = process.ExitCode;
            Status = exitCode == 0 ? TerminalStatus.Success : TerminalStatus.Error;
            AppendLog($"[INFO] [{Name}] Finished with exit code: {exitCode}");
            
            return exitCode;
        }
        catch (Exception ex)
        {
            Status = TerminalStatus.Error;
            AppendLog($"[CRITICAL] [{Name}] {ex.Message}");
            return -1;
        }
    }

    public void ClearLogs()
    {
        _logs.Clear();
        LogReceived?.Invoke(string.Empty);
    }

    private void AppendLog(string message)
    {
        _logs.AppendLine(message);
        
        // Notify local instance listeners
        LogReceived?.Invoke(message);
        
        // Notify global listeners with source name
        OnAnyLogReceived?.Invoke(Name, message);
    }
}
