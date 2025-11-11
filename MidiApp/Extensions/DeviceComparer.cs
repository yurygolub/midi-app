using System.Collections.Generic;
using MidiApp.Models;

namespace MidiApp.Extensions;

internal class DeviceComparer : IEqualityComparer<Device>
{
    public bool Equals(Device x, Device y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        {
            return false;
        }

        return x.Index == y.Index && x.Name == y.Name;
    }

    public int GetHashCode(Device obj)
    {
        if (ReferenceEquals(obj, null))
        {
            return 0;
        }

        int hashDeviceName = obj.Name == null ? 0 : obj.Name.GetHashCode();

        int hashDeviceIndex = obj.Index.GetHashCode();

        return hashDeviceName ^ hashDeviceIndex;
    }
}
