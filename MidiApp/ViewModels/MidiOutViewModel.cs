using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using MidiApp.Commands;
using MidiApp.Extensions;
using MidiApp.Models;

#pragma warning disable CS0067

namespace MidiApp.ViewModels
{
    public class MidiOutViewModel : INotifyPropertyChanged
    {
        private readonly MidiOutModel model;

        private ICommand clearCommand;
        private ICommand startPlaybackCommand;
        private ICommand openFileCommand;

        private string filePath;

        public MidiOutViewModel(MidiOutModel midiOutModel)
        {
            this.model = midiOutModel ?? throw new ArgumentNullException(nameof(midiOutModel));

            this.CheckDevices();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool StartButtonEnabled { get; set; } = true;

        public IEnumerable<Device> Devices { get; set; }

        public Device SelectedDevice { get; set; }

        public string Output { get; set; }

        public ICommand ClearCommand =>
            this.clearCommand ??= new ActionCommand(() =>
            {
                this.Output = string.Empty;
            });

        public ICommand StartPlaybackCommand =>
            this.startPlaybackCommand ??= new ActionCommand(async () =>
            {
                this.ToggleControls();
                this.CheckDevices();

                try
                {
                    await this.model.StartPlayingAsync(this.SelectedDevice.Index, this.filePath);
                }
                finally
                {
                    this.ToggleControls();
                    this.CheckDevices();
                }
            });

        public ICommand OpenFileCommand =>
            this.openFileCommand ??= new ActionCommand(() =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Midi files (*.mid)|*.mid|All files (*.*)|*.*",
                    RestoreDirectory = true,
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    this.filePath = openFileDialog.FileName;
                }
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
        }
    }
}
