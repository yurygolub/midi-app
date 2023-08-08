namespace MidiApp.Models
{
    public class Device
    {
        public int Index { get; init; }

        public string Name { get; init; }

        public static bool operator ==(Device left, Device right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Index == right.Index && left.Name == right.Name;
        }

        public static bool operator !=(Device left, Device right) => !(left == right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is Device device)
            {
                return this == device;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashDeviceName = this.Name == null ? 0 : this.Name.GetHashCode();

            int hashDeviceIndex = this.Index.GetHashCode();

            return hashDeviceName ^ hashDeviceIndex;
        }
    }
}
