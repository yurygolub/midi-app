using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MidiApp.Infrastructure;
using MidiApp.Models;
using MidiApp.ViewModels;
using MidiApp.Views;
using NLog.Extensions.Logging;

namespace MidiApp;

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
