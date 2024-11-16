using System;
using System.Windows.Input;
using SniffSharp.Avalonia.ViewModels;

namespace SniffSharp.Avalonia.Commands;

public class ScanIpCommand(IpScannerViewModel viewModel) : ICommand
{
    public event EventHandler? CanExecuteChanged;
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        
    }
}