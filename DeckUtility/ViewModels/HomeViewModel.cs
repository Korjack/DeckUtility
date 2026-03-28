using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;
using DeckUtility.Interfaces;
using DeckUtility.Services;

namespace DeckUtility.ViewModels;

public class HomeViewModel : ContentViewModelBase
{
    private string _statusText = "Ready";
    private readonly ITerminalService _terminalService;

    public override string DisplayName => "Home";

    public string StatusText
    {
        get => _statusText;
        private set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    public ICommand RunTestCommand { get; }

    public HomeViewModel(ITerminalService terminalService)
    {
        _terminalService = terminalService;
        _terminalService.StatusChanged += status => StatusText = $"Status: {status}";
        
        RunTestCommand = ReactiveCommand.CreateFromTask(async () => 
        {
            await _terminalService.ExecuteCommandAsync("ls -la && sleep 2 && echo 'Done!'");
        });
    }
}