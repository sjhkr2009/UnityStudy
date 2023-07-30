using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : IItemHandler {
    public abstract AbilityIndex Index { get; }

    protected AbilityData _data; 
    public AbilityData Data => _data ??= AbilityDataContainer.GetDataOrDefault(Index);
    public virtual int Level { get; set; }

    public virtual void Initialize(ItemController controller) {
        Level = 1;
    }

    public virtual void Upgrade() {
        Level++;
    }
    public virtual void OnEveryFrame(float deltaTime) { }
    public virtual void Abandon() { }
    public virtual void OnChangeOtherAbility(AbilityBase changedAbility) { }
}
