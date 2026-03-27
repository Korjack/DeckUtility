using DeckUtility.Interfaces;

namespace DeckUtility.ViewModels;

public abstract class ContentViewModelBase : ViewModelBase, IContent
{
    public abstract string DisplayName { get; }
}