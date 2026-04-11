public static class ShopData
{
    public static bool[] lockedSlots;

    public static void Init(int size)
    {
        if (lockedSlots == null || lockedSlots.Length != size)
        {
            lockedSlots = new bool[size];
        }
    }
}