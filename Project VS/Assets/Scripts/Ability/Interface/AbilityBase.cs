public abstract class AbilityBase {
    public abstract AbilityIndex Index { get; }

    protected AbilityData _data; 
    public AbilityData Data => _data ??= AbilityDataContainer.GetDataOrDefault(Index);
    public virtual int Level { get; set; }

    public virtual void Initialize(AbilityController controller) {
        Level = 1;
    }

    public virtual void Upgrade() {
        Level++;
    }
    public virtual void Abandon() { }
    public virtual void OnChangeOtherAbility(AbilityBase changedAbility) { }
}
