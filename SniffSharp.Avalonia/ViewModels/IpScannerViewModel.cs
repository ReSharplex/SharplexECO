using System.Windows.Input;
using SniffSharp.Avalonia.Commands;

namespace SniffSharp.Avalonia.ViewModels;

public class IpScannerViewModel : ViewModelBase
{
    public ICommand ScanIpCommand { get; set; }
    
    public IpScannerViewModel()
    {
        ScanIpCommand = new ScanIpCommand(this);
    }
}