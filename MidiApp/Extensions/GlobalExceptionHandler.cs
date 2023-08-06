using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;

namespace MidiApp.Extensions
{
    internal class GlobalExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogUnhandledException(e.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException");
            };

            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };

            void LogUnhandledException(Exception exception, string source)
            {
                object name = null, version = null;
                try
                {
                    AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                    name = assemblyName.Name;
                    version = assemblyName.Version;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Exception in LogUnhandledException");
                }
                finally
                {
                    this.logger.LogError(exception, "Unhandled exception in {Name} v{Version}. Source: {Source}", name, version, source);
                }
            }
        }
    }
}
