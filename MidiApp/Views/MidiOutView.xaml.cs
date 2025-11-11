using System;
using System.Windows.Controls;
using MidiApp.Wpf.ViewModels;

namespace MidiApp.Wpf.Views;

/// <summary>
/// Interaction logic for MidiOutView.xaml.
/// </summary>
public partial class MidiOutView : UserControl
{
    public MidiOutView(MidiOutViewModel viewModel)
    {
        this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.InitializeComponent();
    }

    public MidiOutViewModel ViewModel { get; }
}
