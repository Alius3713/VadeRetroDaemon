namespace _Project.Scripts.Core
{
    public static class WindowsInputLock
    {
        public static bool Occupied { get; private set; }

        public static void SetOccupied(bool value)
        {
            Occupied = value;
        }
    }
}
