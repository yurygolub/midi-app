using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MidiApp.Commands;
using MidiApp.Models;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;

        private ICommand startCommand;
        private ICommand stopCommand;
        private ICommand clearCommand;

        public MainWindowViewModel(MainWindowModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            this.InDevices = this.model.GetMidiInDevices();
            this.SelectedInDevice = this.InDevices.FirstOrDefault();

            this.OutDevices = this.model.GetMidiOutDevices();
            this.SelectedOutDevice = this.OutDevices.FirstOrDefault();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool StartButtonEnabled { get; set; } = true;

        public bool StopButtonEnabled { get; set; }

        public IEnumerable<Device> InDevices { get; set; }

        public Device SelectedInDevice { get; set; }

        public IEnumerable<Device> OutDevices { get; set; }

        public Device SelectedOutDevice { get; set; }

        public string OutputText { get; set; }

        public ICommand StartCommand =>
            this.startCommand ??= new ActionCommand(() =>
            {
                this.StartButtonEnabled = false;
                this.StopButtonEnabled = true;

                this.model.StartListening(this.SelectedOutDevice.Index);
            });

        public ICommand StopCommand =>
            this.stopCommand ??= new ActionCommand(() =>
            {
                this.model.StopListening();

                this.StartButtonEnabled = true;
                this.StopButtonEnabled = false;
            });

        public ICommand ClearCommand =>
            this.clearCommand ??= new ActionCommand(() =>
            {
                this.OutputText = string.Empty;
            });
    }
}
