using Avalonia.Controls;
using ReactiveUI;
using SniffSharp.Avalonia.Views;

namespace SniffSharp.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private UserControl _selectedView;

    public UserControl SelectedView
    {
        get => _selectedView;
        set => this.RaiseAndSetIfChanged(ref _selectedView, value);
    }

    public MainWindowViewModel()
    {
        SelectedView = new IpScannerView();
    }
}