namespace _Project.Scripts.Core
{
    public static class InputLock
    {
        public static bool Occupied { get; private set; }

        public static void SetOccupied(bool value)
        {
            Occupied = value;
        }
    }
}
