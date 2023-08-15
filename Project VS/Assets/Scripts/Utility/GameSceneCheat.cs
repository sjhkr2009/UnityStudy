using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameSceneCheat : MonoBehaviour {
    void OnEnable() {
        if (!Define.IsTesting) gameObject.SetActive(false);
    }
    
    [Button("능력 추가하기")]
    public void GetAbility(AbilityIndex abilityIndex) {
        if (!Define.IsTesting) return;
        if (abilityIndex == AbilityIndex.None) return;

        GameManager.Ability.AddOrUpgradeItem(abilityIndex);
    }
}
