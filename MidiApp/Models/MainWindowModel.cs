using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MidiApp.Wpf.Extensions;
using MidiApp.Wpf.Infrastructure;

namespace MidiApp.Wpf.Models;

public class MainWindowModel
{
    private readonly ObservableCollection<Tab> tabCollection = new ();

    public MainWindowModel(IEnumerable<Tab> tabs)
    {
        _ = tabs ?? throw new ArgumentNullException(nameof(tabs));

        this.TabPublicCollection = new ReadOnlyObservableCollection<Tab>(this.tabCollection);
        this.tabCollection.CollectionChanged += this.Tabs_CollectionChanged;

        foreach (Tab tab in tabs)
        {
            this.tabCollection.Add(tab);
        }
    }

    public ReadOnlyObservableCollection<Tab> TabPublicCollection { get; }

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

    private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                (e.NewItems[0] as Tab).CloseRequested += this.Tab_CloseRequested;
                break;

            case NotifyCollectionChangedAction.Remove:
                (e.OldItems[0] as Tab).CloseRequested -= this.Tab_CloseRequested;
                break;
        }
    }

    private void Tab_CloseRequested(object sender, EventArgs e)
    {
        this.tabCollection.Remove(sender as Tab);
    }
}
