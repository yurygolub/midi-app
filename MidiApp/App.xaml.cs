using System.Windows;
using MidiApp.Models;
using MidiApp.ViewModels;
using MidiApp.Views;

namespace MidiApp
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public static MainWindowViewModel MainWindowViewModel { get; } = new (new MainWindowModel());

        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow(MainWindowViewModel).Show();

            base.OnStartup(e);
        }
    }
}
