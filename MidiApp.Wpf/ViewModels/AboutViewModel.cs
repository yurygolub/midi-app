using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MidiApp.Wpf.Infrastructure;

namespace MidiApp.Wpf.ViewModels;

public class AboutViewModel : ObservableObject
{
    private readonly ILogger<AboutViewModel> logger;

    public AboutViewModel(ILogger<AboutViewModel> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Version version = assembly.GetName().Version;
            var buildDate = new DateTime(2000, 1, 1)
                .AddDays(version.Build)
                .AddSeconds(version.Revision * 2);

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.Version = version.ToString();
            this.ProductVersion = fileVersionInfo.ProductVersion;
            this.BuildDate = buildDate.ToString();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled exception");
        }
    }

    public string Version { get; }

    public string ProductVersion { get; }

    public string BuildDate { get; }
}
