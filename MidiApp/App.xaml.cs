using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MidiApp.Extensions;
using MidiApp.Views;
using NLog.Extensions.Logging;

namespace MidiApp
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private readonly Startup startup = new ();

        protected override void OnStartup(StartupEventArgs e)
        {
            ConsoleManager.OpenConsole();
            ConsoleManager.HideConsole();

            new ServiceCollection()
                .AddSingleton<GlobalExceptionHandler>()
                .AddLogging(builder =>
                {
                    builder
                        .AddConsole()
                        .AddNLog(this.startup.Configuration);
                })
                .BuildServiceProvider()
                .GetRequiredService<GlobalExceptionHandler>()
                .SetupExceptionHandling();

            this.MainWindow = this.startup
                .ConfigureServices(new ServiceCollection())
                .BuildServiceProvider()
                .GetRequiredService<MainWindow>();

            this.MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ConsoleManager.CloseConsole();

            base.OnExit(e);
        }
    }
}
