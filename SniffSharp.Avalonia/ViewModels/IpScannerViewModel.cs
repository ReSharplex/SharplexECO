using System.Windows.Input;
using ReactiveUI;
using SniffSharp.Avalonia.Commands;

namespace SniffSharp.Avalonia.ViewModels;

public class IpScannerViewModel : ViewModelBase
{
    public ICommand ScanIpCommand { get; set; }
    
    public IpScannerViewModel()
    {
        ScanIpCommand = new ScanIpCommand(this);
    }

    private string _continent;
    public string Continent
    {
        get => _continent;
        set => this.RaiseAndSetIfChanged(ref _continent, value);
    }
    
    private string _country;
    public string Country
    {
        get => _country;
        set => this.RaiseAndSetIfChanged(ref _country, value);
    }

    private string _city;
    public string City
    {
        get => _city;
        set => this.RaiseAndSetIfChanged(ref _city, value);
    }

    private string _isp;
    public string Isp
    {
        get => _isp;
        set => this.RaiseAndSetIfChanged(ref _isp, value);
    }

    private string _org;
    public string Org
    {
        get => _org;
        set => this.RaiseAndSetIfChanged(ref _org, value);
    }

    private string _as;
    public string As
    {
        get => _as;
        set => this.RaiseAndSetIfChanged(ref _as, value);
    }
}