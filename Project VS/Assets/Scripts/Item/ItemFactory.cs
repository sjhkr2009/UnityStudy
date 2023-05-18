using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFactory {
    public static ItemBase Create(ItemIndex itemIndex) {
        ItemBase item = itemIndex switch {
            ItemIndex.WeaponAutoGun => new FireBulletWeapon(),
            ItemIndex.WeaponSpinAround => new SpinnerWeapon(),
            ItemIndex.NormalShoes => new NormalShoes(),
            ItemIndex.NormalGlove => new NormalGlove(),
            _ => null
        };
        if (item == null) Debugger.Error($"[ItemFactory.Create] Cannot find {itemIndex} Implementation!");
        
        return item;
    }
}
