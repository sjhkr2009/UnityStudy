using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Composite {
    public class ExampleInitializer_Composite : MonoBehaviour {
        [SerializeField] private InventoryHandler inventory;

        private List<BaseItemInfo> playerItemInfos = new List<BaseItemInfo>() {
            new ItemInfoSword(),
            new ItemInfoElixir(5),
            new ItemInfoBag() {InnerItems = { new ItemInfoBag() {InnerItems = { new ItemInfoElixir(5), new ItemInfoBow() }},
                new ItemInfoRedPotion(10), new ItemInfoBluePotion(10) }},
        };

        private void Start() {
            inventory.Initialize(playerItemInfos);
        }

        #region Editor Code

        [ShowInInspector, ReadOnly]
        private List<(string, int)> inspectorInfo {
            get {
                var ret = new List<(string, int)>();
                foreach (var playerItemInfo in playerItemInfos) {
                    int count = playerItemInfo is PotionItemInfo potion ? potion.Count : 1;
                    ret.Add((playerItemInfo.Name, count));
                }

                return ret;
            }
        }

        #endregion
    }
}
