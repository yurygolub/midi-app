using System.ComponentModel;
using System.Runtime.CompilerServices;

#pragma warning disable CS0067 // The event is never used

namespace MidiApp.Infrastructure;

public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(backingField, value))
        {
            return false;
        }

        backingField = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
}
