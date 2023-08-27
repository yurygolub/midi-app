using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MidiApp.ViewModels;
using NAudio.Midi;

namespace MidiApp.Models
{
    public class MidiOutModel
    {
        private readonly IServiceProvider serviceProvider;
        private MidiOutViewModel midiOutViewModel;

        public MidiOutModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IEnumerable<Device> GetDevices()
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

        public async Task StartPlayingAsync(int deviceIndex, string filePath, CancellationToken token)
        {
            this.midiOutViewModel ??= this.serviceProvider.GetRequiredService<MidiOutViewModel>();

            var midiFile = new MidiFile(filePath, false);
            this.midiOutViewModel.Output += $"Format {midiFile.FileFormat}, Tracks {midiFile.Tracks}, Delta Ticks Per Quarter Note {midiFile.DeltaTicksPerQuarterNote}{Environment.NewLine}";

            TimeSignatureEvent timeSignature = midiFile.Events[0].OfType<TimeSignatureEvent>().FirstOrDefault();

            using var midiOut = new MidiOut(deviceIndex);
            int absoluteTime = 0;

            for (int i = 0; i < midiFile.Tracks; i++)
            {
                foreach (MidiEvent midiEvent in midiFile.Events[i])
                {
                    if (!MidiEvent.IsNoteOff(midiEvent))
                    {
                        this.midiOutViewModel.Output += $"{ToMeasuresBeatsTicks(midiEvent.AbsoluteTime, midiFile.DeltaTicksPerQuarterNote, timeSignature)} {midiEvent}{Environment.NewLine}";
                    }

                    if (midiEvent.AbsoluteTime > absoluteTime)
                    {
                        await Task.Delay((int)midiEvent.AbsoluteTime - absoluteTime, token);
                        absoluteTime = (int)midiEvent.AbsoluteTime;
                    }

                    midiOut.Send(midiEvent.GetAsShortMessage());
                }
            }
        }

        private static string ToMeasuresBeatsTicks(long eventTime, int ticksPerQuarterNote, TimeSignatureEvent timeSignature)
        {
            int beatsPerBar = timeSignature?.Numerator ?? 4;

            int ticksPerBar = timeSignature == null
                ? ticksPerQuarterNote * 4
                : timeSignature.Numerator * ticksPerQuarterNote * 4 / (1 << timeSignature.Denominator);

            int ticksPerBeat = ticksPerBar / beatsPerBar;
            long bar = 1 + (eventTime / ticksPerBar);
            long beat = 1 + ((eventTime % ticksPerBar) / ticksPerBeat);
            long tick = eventTime % ticksPerBeat;

            return $"{bar}:{beat}:{tick}";
        }
    }
}
