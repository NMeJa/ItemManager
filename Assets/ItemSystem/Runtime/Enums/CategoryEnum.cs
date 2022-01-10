using System;

namespace GoblinAdventures.ItemSystem.Runtime
{
    [Flags]
    public enum CategoryEnum
    {
        Weapon = 1 << 1,
        Armor = 1 << 2,
        Consumable = 1 << 3,
        Quest = 1 << 4,
        Key = 1 << 5,
        Miscellaneous = 1 << 6,
    }
}