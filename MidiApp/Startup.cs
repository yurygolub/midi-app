using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MidiApp.Wpf.Infrastructure;
using MidiApp.Wpf.Models;
using MidiApp.Wpf.ViewModels;
using MidiApp.Wpf.Views;
using NLog.Extensions.Logging;

namespace MidiApp.Wpf;

public class Startup
{
    public IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", true, true)
        .Build();

    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSingleton<MainWindow>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindowModel>()
            .AddSingleton<MidiInView>()
            .AddSingleton<MidiInViewModel>()
            .AddSingleton<MidiInModel>()
            .AddSingleton<MidiOutView>()
            .AddSingleton<MidiOutViewModel>()
            .AddSingleton<MidiOutModel>()
            .AddSingleton<Tab, MidiInTab>()
            .AddSingleton<Tab, MidiOutTab>()
            .AddTransient<AboutView>()
            .AddSingleton<AboutViewModel>()
            .AddLogging(builder =>
            {
                builder
                    .AddConsole()
                    .AddNLog(this.Configuration);
            });
    }
}
