using System;
using System.Collections.Generic;
using System.ComponentModel;
using NAudio.Midi;

namespace MidiApp.Models
{
    public class MainWindowModel
    {
        private MidiIn midiIn;

        public IEnumerable<Device> GetMidiInDevices()
        {
            var devices = new List<Device>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                MidiInCapabilities devInfo = MidiIn.DeviceInfo(i);
                devices.Add(new Device
                {
                    Index = i,
                    Name = devInfo.ProductName,
                });
            }

            return devices;
        }

        public IEnumerable<Device> GetMidiOutDevices()
        {
            var devices = new List<Device>();
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                MidiOutCapabilities devInfo = MidiOut.DeviceInfo(i);
                devices.Add(new Device
                {
                    Index = i,
                    Name = devInfo.ProductName,
                });
            }

            return devices;
        }

        public void StartListening(int deviceIndex)
        {
            this.midiIn = new MidiIn(deviceIndex);
            this.midiIn.MessageReceived += this.MidiIn_MessageReceived;
            this.midiIn.ErrorReceived += this.MidiIn_ErrorReceived;
            this.midiIn.Start();
        }

        public void StopListening()
        {
            this.midiIn.Stop();
            this.midiIn.Dispose();
        }

        private void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            App.MainWindowViewModel.OutputText += $"Error: Time {e.Timestamp} Message 0x{e.RawMessage:X8} Event {e.MidiEvent}{Environment.NewLine}";
        }

        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent.CommandCode != MidiCommandCode.TimingClock
                && e.MidiEvent.CommandCode != MidiCommandCode.AutoSensing)
            {
                App.MainWindowViewModel.OutputText += $"Message: Time {e.Timestamp} Message 0x{e.RawMessage:X8} Event {e.MidiEvent}{Environment.NewLine}";
            }
        }
    }
}
