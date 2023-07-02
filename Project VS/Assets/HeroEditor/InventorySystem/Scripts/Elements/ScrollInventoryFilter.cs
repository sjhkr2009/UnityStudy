using System.Collections.Generic;
using Assets.HeroEditor.InventorySystem.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.InventorySystem.Scripts.Elements
{
    public class ScrollInventoryFilter : MonoBehaviour
    {
        public ScrollInventory ScrollInventory;
        public Toggle Weapon;
        public Toggle Armor;
        public Toggle Helmet;
        public Toggle Shield;

        public void OnSelect(bool value)
        {
            var types = new List<Enums.ItemType>();

            if (Weapon.isOn) types.Add(Enums.ItemType.Weapon);
            if (Armor.isOn) types.Add(Enums.ItemType.Armor);
            if (Helmet.isOn) types.Add(Enums.ItemType.Helmet);
            if (Shield.isOn) types.Add(Enums.ItemType.Shield);
            
            ScrollInventory.SetTypeFilter(types);
        }
    }
}