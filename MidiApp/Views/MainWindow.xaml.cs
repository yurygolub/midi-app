using System;
using System.Windows;
using MidiApp.ViewModels;

namespace MidiApp.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.InitializeComponent();
    }

    public MainWindowViewModel ViewModel { get; }
}
