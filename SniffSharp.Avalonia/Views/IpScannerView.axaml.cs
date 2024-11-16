using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SniffSharp.Avalonia.ViewModels;

namespace SniffSharp.Avalonia.Views;

public partial class IpScannerView : UserControl
{
    public IpScannerView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.Text = string.Empty;
        }
    }

    [Obsolete("Obsolete")]
    private void InputElement_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (e.Key != Key.Enter) return;
        
        var dataContext = textBox?.DataContext as IpScannerViewModel;
        dataContext?.ScanIpCommand?.Execute(textBox?.Text);

        var topLevel = TopLevel.GetTopLevel(this);
        topLevel?.FocusManager?.ClearFocus();
    }
}