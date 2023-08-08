using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MidiApp.Commands;
using MidiApp.Extensions;
using MidiApp.Models;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MidiInViewModel : INotifyPropertyChanged
    {
        private readonly MidiInModel model;

        private ICommand startCommand;
        private ICommand stopCommand;
        private ICommand clearCommand;

        public MidiInViewModel(MidiInModel midiInModel)
        {
            this.model = midiInModel ?? throw new ArgumentNullException(nameof(midiInModel));

            this.CheckDevices();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool StartButtonEnabled { get; set; } = true;

        public bool StopButtonEnabled { get; set; }

        public IEnumerable<Device> Devices { get; set; }

        public Device SelectedDevice { get; set; }

        public string Output { get; set; }

        public ICommand StartCommand =>
            this.startCommand ??= new ActionCommand(() =>
            {
                this.ToggleControls();
                this.CheckDevices();

                try
                {
                    this.model.StartListening(this.SelectedDevice.Index);
                }
                catch
                {
                    this.ToggleControls();
                    this.CheckDevices();
                    this.model.StopListening();
                    throw;
                }
            });

        public ICommand StopCommand =>
            this.stopCommand ??= new ActionCommand(() =>
            {
                this.model.StopListening();
                this.ToggleControls();
                this.CheckDevices();
            });

        public ICommand ClearCommand =>
            this.clearCommand ??= new ActionCommand(() =>
            {
                this.Output = string.Empty;
            });

        private void CheckDevices()
        {
            var devices = this.model.GetDevices();
            if (this.Devices is null || !this.Devices.SequenceEqual(devices, new DeviceComparer()))
            {
                this.Devices = devices;
                if (!devices.Any(dev => dev == this.SelectedDevice))
                {
                    this.SelectedDevice = devices.FirstOrDefault();
                }
            }
        }

        private void ToggleControls()
        {
            this.StartButtonEnabled = !this.StartButtonEnabled;
            this.StopButtonEnabled = !this.StopButtonEnabled;
        }
    }
}
