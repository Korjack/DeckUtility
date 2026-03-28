using System.Linq;
using ReactiveUI;
using DeckUtility.Interfaces;
using DeckUtility.Services;

namespace DeckUtility.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ContentViewModelBase _currentPage;
    // Specific service for Home/General tasks
    private readonly ITerminalService _homeTerminal = new TerminalService("Home");

    private readonly ContentViewModelBase[] _contentPages;
    
    public MainWindowViewModel()
    {
        var logViewModel = new LogViewModel();
        
        // Option 1: Log only HomeTerminal
        _homeTerminal.LogReceived += logViewModel.AppendLog;
        
        // Option 2 (Future): Log ANY terminal service
        // TerminalService.OnAnyLogReceived += (source, log) => logViewModel.AppendLog($"[{source}] {log}");

        _contentPages =
        [
            new HomeViewModel(_homeTerminal),
            logViewModel
        ];
        
        _currentPage = _contentPages[0];
    }


    public ContentViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }
    public ContentViewModelBase[] ContentPages => _contentPages;
}

