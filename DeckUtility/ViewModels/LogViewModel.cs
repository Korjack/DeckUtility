using ReactiveUI;
using System.Text;

namespace DeckUtility.ViewModels;

public class LogViewModel : ContentViewModelBase
{
    private string _logContent = string.Empty;
    private readonly StringBuilder _stringBuilder = new();

    public override string DisplayName => "LogView";

    public string LogContent
    {
        get => _logContent;
        private set => this.RaiseAndSetIfChanged(ref _logContent, value);
    }

    public void AppendLog(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            _stringBuilder.Clear();
        }
        else
        {
            _stringBuilder.AppendLine(message);
        }
        LogContent = _stringBuilder.ToString();
    }
}