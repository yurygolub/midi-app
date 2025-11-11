using System;
using MidiApp.Views;

namespace MidiApp.Infrastructure;

public class MidiOutTab : Tab
{
    public MidiOutTab(MidiOutView midiOutView)
    {
        this.Header = "Midi out";
        this.CanBeClosed = false;
        this.Content = midiOutView ?? throw new ArgumentNullException(nameof(midiOutView));
    }
}
