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

        public override bool Equals(object obj) => obj is Device device && this == device;

        public override int GetHashCode()
        {
            int hashDeviceName = this.Name == null ? 0 : this.Name.GetHashCode();

            int hashDeviceIndex = this.Index.GetHashCode();

            return hashDeviceName ^ hashDeviceIndex;
        }
    }
}
