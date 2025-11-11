using System;
using MidiApp.Views;

namespace MidiApp.Infrastructure;

public class MidiInTab : Tab
{
    public MidiInTab(MidiInView midiInView)
    {
        this.Header = "Midi in";
        this.CanBeClosed = false;
        this.Content = midiInView ?? throw new ArgumentNullException(nameof(midiInView));
    }
}
