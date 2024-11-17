using Avalonia.Controls;
using ReactiveUI;
using SniffSharp.Avalonia.Views;

namespace SniffSharp.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _selectedView;

    public ViewModelBase SelectedView
    {
        get => _selectedView;
        set => this.RaiseAndSetIfChanged(ref _selectedView, value);
    }

    public MainWindowViewModel()
    {
        SelectedView = new IpScannerViewModel();
    }
}