using System.Linq;
using ReactiveUI;

namespace DeckUtility.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ContentViewModelBase _currentPage;

    private readonly ContentViewModelBase[] _contentPages =
    [
        new HomeViewModel(),
        new LogViewModel()
    ];
    
    public MainWindowViewModel()
    {
        _currentPage = _contentPages[0];
    }

    public ContentViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }
    public ContentViewModelBase[] ContentPages => _contentPages;
}
