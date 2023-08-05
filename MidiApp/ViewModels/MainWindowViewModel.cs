using System;
using System.ComponentModel;
using MidiApp.Models;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;

        public MainWindowViewModel(MainWindowModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
