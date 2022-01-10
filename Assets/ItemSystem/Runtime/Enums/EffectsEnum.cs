using System;

namespace GoblinAdventures.ItemSystem.Runtime
{
    [Flags]
    public enum EffectsEnum
    {
        Lighting = 1 << 1,
        Fire = 1 << 2,
        Ice = 1 << 3,
        Poison = 1 << 4,
        Earth = 1 << 5,
        Wind = 1 << 6,
        Water = 1 << 7,
        Holy = 1 << 8,
        Dark = 1 << 9,
    }
}