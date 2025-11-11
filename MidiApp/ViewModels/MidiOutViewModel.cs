using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.Win32;
using MidiApp.Commands;
using MidiApp.Extensions;
using MidiApp.Infrastructure;
using MidiApp.Models;

namespace MidiApp.ViewModels
{
    public class MidiOutViewModel : ObservableObject
    {
        private readonly MidiOutModel model;

        private ICommand clearCommand;
        private ICommand startPlaybackCommand;
        private ICommand stopPlaybackCommand;
        private ICommand openFileCommand;

        private CancellationTokenSource cts;
        private string filePath;

        public MidiOutViewModel(MidiOutModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            this.CheckDevices();
        }

        public bool StartButtonEnabled { get; set; } = true;

        public bool StopButtonEnabled { get; set; }

        public IEnumerable<Device> Devices { get; set; }

        public Device SelectedDevice { get; set; }

        public StateModel State => this.model.State;

        public string FileName { get; set; }

        public ICommand ClearCommand =>
            this.clearCommand ??= new ActionCommand(() =>
            {
                this.State.Output = string.Empty;
            });

        public ICommand StartPlaybackCommand =>
            this.startPlaybackCommand ??= new ActionCommand(async () =>
            {
                if (this.filePath is null && !this.OpenFile())
                {
                    return;
                }

                this.ToggleControls();
                this.CheckDevices();

                this.cts = new CancellationTokenSource();

                try
                {
                    await this.model.StartPlayingAsync(this.SelectedDevice.Index, this.filePath, this.cts.Token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    this.ToggleControls();
                    this.CheckDevices();
                }
            });

        public ICommand StopPlaybackCommand =>
            this.stopPlaybackCommand ??= new ActionCommand(() =>
            {
                this.ToggleControls();
                this.CheckDevices();

                try
                {
                    this.cts.Cancel();
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
                this.OpenFile();
            });

        private bool OpenFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Midi files (*.mid)|*.mid|All files (*.*)|*.*",
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                this.filePath = dialog.FileName;
                this.FileName = Path.GetFileName(dialog.FileName);
                return true;
            }

            return false;
        }

        private void CheckDevices()
        {
            var devices = MidiOutModel.GetDevices();
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
