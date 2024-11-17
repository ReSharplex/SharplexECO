using System;
using System.Windows.Input;
using SniffSharp.Avalonia.BLogic;
using SniffSharp.Avalonia.ViewModels;

namespace SniffSharp.Avalonia.Commands;

public class ScanIpCommand : ICommand
{
    private IpScannerViewModel ViewModel { get; init; }
    private IpManager IpManager { get; init; }
    
    public ScanIpCommand(IpScannerViewModel viewModel)
    {
        ViewModel = viewModel;
        IpManager = new IpManager();
    }
    
    public event EventHandler? CanExecuteChanged;
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not string stringIp) return;
        if (string.IsNullOrEmpty(stringIp)) return;
        
        var validation = IpManager.Scan(stringIp, out IpData? ipData);
        if (validation.IsValid)
        {
            ViewModel.Continent = ipData?.Continent;
            ViewModel.Country = ipData?.Country;
            ViewModel.City = ipData?.City;
            ViewModel.Isp = ipData?.Isp;
            ViewModel.Org = ipData?.Org;
            ViewModel.As = ipData?.As;
        }
    }
}