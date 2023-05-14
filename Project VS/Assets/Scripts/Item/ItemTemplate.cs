using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemTemplate {
    public static ItemBase GetItem(ItemIndex itemIndex) {
        ItemBase item = itemIndex switch {
            ItemIndex.WeaponAutoGun => new FireBulletWeapon(),
            ItemIndex.WeaponSpinAround => new SpinnerWeapon(),
            ItemIndex.NormalShoes => new NormalShoes(),
            ItemIndex.NormalGlove => new NormalGlove(),
            _ => null
        };
        if (item == null) Debugger.Error($"[ItemBase.Find] Cannot find {itemIndex} Implementation!");
        
        return item;
    }
}
