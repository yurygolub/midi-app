using System;
using MidiApp.Wpf.Views;

namespace MidiApp.Wpf.Infrastructure;

public class MidiInTab : Tab
{
    public MidiInTab(MidiInView midiInView)
    {
        this.Header = "Midi in";
        this.CanBeClosed = false;
        this.Content = midiInView ?? throw new ArgumentNullException(nameof(midiInView));
    }
}
