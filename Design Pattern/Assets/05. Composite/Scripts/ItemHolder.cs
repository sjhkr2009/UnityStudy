using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Composite {
    public class ItemHolder : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text countText;

        private BaseItemInfo itemInfo;
        public void SetItem(BaseItemInfo itemInfo) {
            this.itemInfo = itemInfo;
            
            if (itemInfo == null) {
                itemImage.enabled = false;
                countText.text = string.Empty;
                return;
            }

            itemImage.enabled = true;
            itemImage.sprite = Resources.Load<Sprite>(itemInfo.TextureName);
            countText.text = (itemInfo is PotionItemInfo potion) ? potion.Count.ToString() : string.Empty;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (itemInfo == null) return;
            
            itemInfo.OnSelect();

            if (itemInfo is ItemInfoBag bag) {
                var inven = Instantiate(Resources.Load<InventoryHandler>(nameof(InventoryHandler)), transform.parent);
                inven.Initialize(bag.InnerItems, 4);
                inven.transform.position = transform.position;
            }
        }
    }
}
