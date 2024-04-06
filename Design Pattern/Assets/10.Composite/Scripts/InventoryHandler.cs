using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Composite {
    public class InventoryHandler : MonoBehaviour {
        [SerializeField] private int capacity = 8;
        [SerializeField] private Transform root;

        private List<ItemHolder> itemHolders = new List<ItemHolder>();
        private static ItemHolder loadedItemHolder;

        public void Initialize(List<BaseItemInfo> itemInfos) {
            if (!loadedItemHolder) loadedItemHolder = Resources.Load<ItemHolder>(nameof(ItemHolder));
            
            for (int i = 0; i < capacity; i++) {
                itemHolders.Add(Instantiate(loadedItemHolder, root));
            }

            int count = Mathf.Min(itemInfos.Count, capacity);
            for (int i = 0; i < count; i++) {
                itemHolders[i].SetItem(itemInfos[i]);
            }
        }

        public void Initialize(List<BaseItemInfo> itemInfos, int capacity) {
            this.capacity = capacity;
            Initialize(itemInfos);
        }
    }
}
