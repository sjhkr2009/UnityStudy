[System.Flags]
public enum AbilityType {
    Unknown = 0,
    StatModifier = 1,
    Passive = 1 << 1,
    Active = 1 << 2,
    Hybrid = Passive | Active,
}
