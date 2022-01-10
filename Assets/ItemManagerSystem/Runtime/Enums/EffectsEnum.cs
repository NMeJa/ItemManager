namespace ItemManagerSystem.Enums
{
    [System.Flags]
    public enum EffectsEnum
    {
        Lightning = 1 << 1,
        Fire = 1 << 2,
        Ice = 1 << 3,
        Poison = 1 << 4,
        
    }
}
