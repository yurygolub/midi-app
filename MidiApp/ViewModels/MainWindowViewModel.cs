using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MidiApp.Commands;
using MidiApp.Infrastructure;
using MidiApp.Models;
using MidiApp.Views;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;
        private readonly IServiceProvider serviceProvider;

        private AboutView aboutView;

        private ICommand consoleVisibilityCommand;
        private ICommand aboutCommand;

        public MainWindowViewModel(MainWindowModel model, IServiceProvider serviceProvider)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<Tab> TabCollection => this.model.TabPublicCollection;

        public ICommand ConsoleVisibilityCommand =>
            this.consoleVisibilityCommand ??= new ActionCommand(MainWindowModel.SwitchConsoleVisibility);

        public ICommand AboutCommand =>
            this.aboutCommand ??= new ActionCommand(() =>
            {
                if (this.aboutView is null)
                {
                    this.aboutView = this.serviceProvider.GetRequiredService<AboutView>();
                    this.aboutView.Closed += (o, e) => this.aboutView = null;
                    this.aboutView.Show();
                }

                if (!this.aboutView.IsFocused)
                {
                    this.aboutView.Focus();
                }

                if (this.aboutView.WindowState == WindowState.Minimized)
                {
                    this.aboutView.WindowState = WindowState.Normal;
                }
            });
    }
}
