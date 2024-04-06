using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Composite {
    public enum ItemType {
        Weapon,
        Potion,
        Bag
    }
    interface IItemInfo {
        ItemType Type { get; }
        string Name { get; }
        string TextureName { get; }
        string Description { get; }
        void OnSelect();
    }
    public abstract class BaseItemInfo : IItemInfo {
        public abstract ItemType Type { get; }
        public abstract string Name { get; }
        public abstract string TextureName { get; }
        public abstract string Description { get; }

        public virtual void OnSelect() {
            Debug.Log($"<color=lime>아이템 선택: {Name}</color>");
        }
    }
    
    public abstract class WeaponItemInfo : BaseItemInfo {
        public override ItemType Type => ItemType.Weapon;
        public abstract float AttackDamage { get; protected set; }
        public abstract float Range { get; protected set; }
        
    }

    public class ItemInfoSword : WeaponItemInfo {
        public override string Name => "검";
        public override string TextureName => "sword";
        public override string Description => "근접 공격용 무기";
        public override float AttackDamage { get; protected set; } = 50;
        public override float Range { get; protected set; } = 125;
    }
    public class ItemInfoBow : WeaponItemInfo {
        public override string Name => "활";
        public override string TextureName => "bow";
        public override string Description => "원거리 공격용 무기";
        public override float AttackDamage { get; protected set; } = 30;
        public override float Range { get; protected set; } = 600;
    }
    
    public abstract class PotionItemInfo : BaseItemInfo {
        public PotionItemInfo(int count) {
            Count = count;
        }
        public override ItemType Type => ItemType.Potion;
        public virtual int Count { get; set; }
    }

    public class ItemInfoRedPotion : PotionItemInfo {
        public override string Name => "빨간 포션";
        public override string TextureName => "potion_red";
        public override string Description => "체력을 50 회복시킨다.";
        public ItemInfoRedPotion(int count) : base(count) { }
    }
    public class ItemInfoBluePotion : PotionItemInfo {
        public override string Name => "파란 포션";
        public override string TextureName => "potion_blue";
        public override string Description => "마나를 50 회복시킨다.";
        public ItemInfoBluePotion(int count) : base(count) { }
    }
    public class ItemInfoElixir : PotionItemInfo {
        public override string Name => "엘릭서";
        public override string TextureName => "potion_green";
        public override string Description => "체력과 마나를 100% 회복시킨다.";
        public ItemInfoElixir(int count) : base(count) { }
    }
    
    public abstract class BagItemInfo<T> : BaseItemInfo where T : BaseItemInfo {
        public override ItemType Type => ItemType.Bag;
        public abstract int Capacity { get; }
        public virtual List<T> InnerItems { get; } = new List<T>();
        
        public override void OnSelect() {
            base.OnSelect();
            foreach (var innerItem in InnerItems) {
                Debug.Log($"<color=cyan>내부 아이템: {innerItem.Name}</color>");
            }
        }
    }

    public class ItemInfoBag : BagItemInfo<BaseItemInfo> {
        public override string Name => "4칸 가방";
        public override string TextureName => "bag";
        public override string Description => "아이템을 보관할 수 있는 가방";
        public override int Capacity => 4;
        public override List<BaseItemInfo> InnerItems { get; } = new List<BaseItemInfo>(4);
    }
}
