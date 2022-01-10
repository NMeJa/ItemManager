namespace ItemManagerSystem.Enums
{
    [System.Flags]
    public enum CategoryEnum
    {
        Weapon = 1 << 1,
        Armor = 1 << 2,
        Accessory = 1 << 3,
        Consumable = 1 << 4,
        Quest = 1 << 5,
        Key = 1 << 6,
        Misc = 1 << 7,
    }
}