namespace CSharPet
{
    // Constants.
    static class MyConstants
    {
        public const int MAXVALUE = 10;
        public const int MINVALUE = 0;
        public const int REFRESH_SECS = 5;
        public const int REFRESH_MSECS = REFRESH_SECS * 1000;
        public const int WEAR_SECS = 10;
        public const int WEAR_MSECS = WEAR_SECS * 1000;
    }
    
    // Struct to define the C#Pet status
    enum CSharPetStatus
    {
        Regular,
        Happy,
        Sad,
        Dirty,
        Hungry,
        Dead
    }
}