using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ErganiManager.UI.ViewModels;

namespace ErganiManager.UI.Views;

public partial class BarcodeScanWindow : Window
{
    private TextBox? _scanBox;

    public BarcodeScanWindow()
    {
        InitializeComponent();

        // Focus the input box whenever the window is activated
        Activated += OnWindowActivated;
        DataContextChanged += OnDataContextChanged;
    }

    //protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    //{
    //    base.OnApplyTemplate(e);
    //    FocusScanBox();
    //}

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _scanBox = this.FindControl<TextBox>("ScanInputBox");
        FocusScanBox();
    }

    private void OnWindowActivated(object? sender, System.EventArgs e) =>
        FocusScanBox();

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is not WorkCardScanViewModel vm) return;

        // Re-focus the scan box after every scan (when BarcodeInput clears)
        vm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(WorkCardScanViewModel.BarcodeInput)
                && string.IsNullOrEmpty(vm.BarcodeInput))
            {
                Dispatcher.UIThread.Post(FocusScanBox, DispatcherPriority.Input);
            }

            // Also re-focus when HasResponse becomes false (after ClearResponse)
            if (args.PropertyName == nameof(WorkCardScanViewModel.HasResponse)
                && !vm.HasResponse)
            {
                Dispatcher.UIThread.Post(FocusScanBox, DispatcherPriority.Input);
            }
        };
    }

    private void FocusScanBox()
    {
        if (_scanBox == null)
            _scanBox = this.FindControl<TextBox>("ScanInputBox");

        if (_scanBox != null)
        {
            _scanBox.Focus();
            // Move cursor to end of any existing text
            _scanBox.CaretIndex = _scanBox.Text?.Length ?? 0;
        }
    }
}
