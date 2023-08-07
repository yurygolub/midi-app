using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MidiApp.Extensions;
using MidiApp.ViewModels;
using NAudio.Midi;

namespace MidiApp.Models
{
    public class MainWindowModel
    {
        private readonly IServiceProvider serviceProvider;
        private MainWindowViewModel mainWindowViewModel;
        private MidiIn midiIn;

        public MainWindowModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public static void SwitchConsoleVisibility()
        {
            if (ConsoleManager.IsConsoleVisible())
            {
                ConsoleManager.HideConsole();
            }
            else
            {
                ConsoleManager.ShowConsole();
            }
        }

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
            this.mainWindowViewModel ??= this.serviceProvider.GetRequiredService<MainWindowViewModel>();

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

        public async Task StartPlayingAsync(int deviceIndex, string filePath)
        {
            this.mainWindowViewModel ??= this.serviceProvider.GetRequiredService<MainWindowViewModel>();

            var midiFile = new MidiFile(filePath, false);
            this.mainWindowViewModel.MidiOutOutput += $"Format {midiFile.FileFormat}, Tracks {midiFile.Tracks}, Delta Ticks Per Quarter Note {midiFile.DeltaTicksPerQuarterNote}{Environment.NewLine}";

            TimeSignatureEvent timeSignature = midiFile.Events[0].OfType<TimeSignatureEvent>().FirstOrDefault();

            var midiOut = new MidiOut(deviceIndex);
            int absoluteTime = 0;

            for (int i = 0; i < midiFile.Tracks; i++)
            {
                foreach (MidiEvent midiEvent in midiFile.Events[i])
                {
                    if (!MidiEvent.IsNoteOff(midiEvent))
                    {
                        this.mainWindowViewModel.MidiOutOutput += $"{ToMeasuresBeatsTicks(midiEvent.AbsoluteTime, midiFile.DeltaTicksPerQuarterNote, timeSignature)} {midiEvent}{Environment.NewLine}";
                    }

                    if (midiEvent.AbsoluteTime > absoluteTime)
                    {
                        await Task.Delay((int)midiEvent.AbsoluteTime - absoluteTime);
                        absoluteTime = (int)midiEvent.AbsoluteTime;
                    }

                    midiOut.Send(midiEvent.GetAsShortMessage());
                }
            }

            midiOut.Dispose();
        }

        private static string ToMeasuresBeatsTicks(long eventTime, int ticksPerQuarterNote, TimeSignatureEvent timeSignature)
        {
            int beatsPerBar = timeSignature == null
                ? 4
                : timeSignature.Numerator;

            int ticksPerBar = timeSignature == null
                ? ticksPerQuarterNote * 4
                : timeSignature.Numerator * ticksPerQuarterNote * 4 / (1 << timeSignature.Denominator);

            int ticksPerBeat = ticksPerBar / beatsPerBar;
            long bar = 1 + (eventTime / ticksPerBar);
            long beat = 1 + ((eventTime % ticksPerBar) / ticksPerBeat);
            long tick = eventTime % ticksPerBeat;

            return $"{bar}:{beat}:{tick}";
        }

        private void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            this.mainWindowViewModel.MidiInOutput += $"Error: Time {e.Timestamp} Message 0x{e.RawMessage:X8} Event {e.MidiEvent}{Environment.NewLine}";
        }

        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent.CommandCode != MidiCommandCode.TimingClock
                && e.MidiEvent.CommandCode != MidiCommandCode.AutoSensing)
            {
                this.mainWindowViewModel.MidiInOutput += $"Message: Time {e.Timestamp} Message 0x{e.RawMessage:X8} Event {e.MidiEvent}{Environment.NewLine}";
            }
        }
    }
}
