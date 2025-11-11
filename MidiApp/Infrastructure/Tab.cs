using System;
using System.Windows.Input;
using MidiApp.Commands;

namespace MidiApp.Infrastructure;

public abstract class Tab
{
    private ICommand closeCommand;

    public event EventHandler CloseRequested;

    public string Header { get; set; }

    public object Content { get; set; }

    public ICommand CloseCommand =>
        this.closeCommand ??= new ActionCommand(() => this.CloseRequested?.Invoke(this, EventArgs.Empty));

    public bool CanBeClosed { get; set; } = true;
}
