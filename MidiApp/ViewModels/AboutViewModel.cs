using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Logging;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        private readonly ILogger<AboutViewModel> logger;

        public AboutViewModel(ILogger<AboutViewModel> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                var buildDate = new DateTime(2000, 1, 1)
                    .AddDays(version.Build)
                    .AddSeconds(version.Revision * 2);

                this.Version = version.ToString();
                this.BuildDate = buildDate.ToString();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unhandled exception");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Version { get; }

        public string BuildDate { get; }
    }
}
