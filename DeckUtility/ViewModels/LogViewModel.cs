namespace DeckUtility.ViewModels;

public class LogViewModel : ContentViewModelBase
{
    public override string DisplayName => "LogView";
    public string LogContent { get; } = string.Empty;
}