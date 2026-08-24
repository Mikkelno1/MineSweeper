using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MineSweeper.ViewModels;

namespace MineSweeperz.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    public bool menuVisible => CurrentViewModel is null;

    partial void OnCurrentViewModelChanged(ViewModelBase? value)
    {
        OnPropertyChanged(nameof(menuVisible));
    }

    public MainViewModel() => CurrentViewModel = null;
    
    
    [RelayCommand]
    public void NavigateToGame() => CurrentViewModel = new GameViewModel();
}