using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : IItemHandler {
    public abstract ItemIndex Index { get; }
    public ItemData Data { get; protected set; }
    public virtual int Level { get; set; }

    public virtual void Initialize(ItemController controller) {
        Data = ItemDataContainer.GetDataOrDefault(Index);
        Level = 1;
    }

    public virtual void Upgrade() {
        Level++;
    }
    public virtual void OnEveryFrame(float deltaTime) { }
    public virtual void Abandon() { }
    public virtual void OnChangeOtherItem(ItemBase changedItem) { }
}
