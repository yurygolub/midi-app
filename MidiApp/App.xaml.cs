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
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow(
                new MainWindowViewModel(
                    new MainWindowModel())).Show();

            base.OnStartup(e);
        }
    }
}
