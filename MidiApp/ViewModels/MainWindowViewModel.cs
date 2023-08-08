using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MidiApp.Commands;
using MidiApp.Infrastructure;
using MidiApp.Models;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;

        private ICommand consoleVisibilityCommand;

        public MainWindowViewModel(MainWindowModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<Tab> TabCollection => this.model.TabPublicCollection;

        public ICommand ConsoleVisibilityCommand =>
            this.consoleVisibilityCommand ??= new ActionCommand(MainWindowModel.SwitchConsoleVisibility);
    }
}
