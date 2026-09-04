using Avalonia.Controls;
using Avalonia.Input;
using ErganiManager.UI.ViewModels;

namespace ErganiManager.UI.Views;

public partial class SchedulesView : UserControl
{
    public SchedulesView()
    {
        InitializeComponent();
    }

    private void OnCellPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border { Tag: CalendarCellViewModel cell }
            && DataContext is SchedulesViewModel vm)
        {
            bool ctrl = e.KeyModifiers.HasFlag(KeyModifiers.Control)
                     || e.KeyModifiers.HasFlag(KeyModifiers.Meta); // Cmd on Mac
            vm.OnCellClick(cell, ctrl);
        }
    }
}
