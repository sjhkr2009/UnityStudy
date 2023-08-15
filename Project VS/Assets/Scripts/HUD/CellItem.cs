using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellItem : MonoBehaviour {
    [SerializeField] private Image icon;
    // Start is called before the first frame update
    public void Init() {
        icon.gameObject.SetActive(false);
    }

    public void Init(Sprite iconName) {
        icon.gameObject.SetActive(true);
        icon.sprite = iconName;
    }
}
