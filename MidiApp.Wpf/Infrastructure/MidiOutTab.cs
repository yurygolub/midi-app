using System;
using MidiApp.Wpf.Views;

namespace MidiApp.Wpf.Infrastructure;

public class MidiOutTab : Tab
{
    public MidiOutTab(MidiOutView midiOutView)
    {
        this.Header = "Midi out";
        this.CanBeClosed = false;
        this.Content = midiOutView ?? throw new ArgumentNullException(nameof(midiOutView));
    }
}
