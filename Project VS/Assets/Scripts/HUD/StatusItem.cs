using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatusType {
    AttackPower,
    AttackSpeed,
    MaxHp,
    MoveSpeed,
    Critical
}

public class StatusItem : MonoBehaviour {
    [SerializeField] private Sprite[] icons;
    [SerializeField] private Image[] gauges;
    private Dictionary<StatusType, float[]> gaugeMeasure = new Dictionary<StatusType, float[]>() {
        {StatusType.AttackPower, new float[]{2,6}},
        {StatusType.AttackSpeed, new float[]{1,1.5f}},
        {StatusType.MaxHp, new float[]{200,500}},
        {StatusType.MoveSpeed, new float[]{3,4.5f}},
        {StatusType.Critical, new float[]{20,50}},
    };
    [SerializeField] private StatusType currentType;
    [SerializeField] private Image icon;
    // Start is called before the first frame update
    public void Init() {
        var measure = gaugeMeasure[currentType][0] +
                      (gaugeMeasure[currentType][1] - gaugeMeasure[currentType][0]) / 5;
        
        switch (currentType) {
            case StatusType.AttackPower:
                icon.sprite = icons[0];
                
                for (int i = 0; i < gauges.Length; i++) {
                    if (GameManager.Player.Status.AttackPower >= measure * i) {
                        gauges[i].color = new Color(1, 0.5f, 0.098f);
                    } else {
                        gauges[i].color = new Color(0.2039f, 0.2039f, 0.2039f);
                    }
                }
                break;
            case StatusType.AttackSpeed:
                icon.sprite = icons[1];
                for (int i = 0; i < gauges.Length; i++) {
                    if (GameManager.Player.Status.AttackSpeed >= measure * i) {
                        gauges[i].color = new Color(1, 0.5f, 0.098f);
                    } else {
                        gauges[i].color = new Color(0.2039f, 0.2039f, 0.2039f);
                    }
                }
                break;
            case StatusType.MaxHp:
                icon.sprite = icons[2];
                for (int i = 0; i < gauges.Length; i++) {
                    if (GameManager.Player.Status.MaxHp >= measure * i) {
                        gauges[i].color = new Color(1, 0.5f, 0.098f);
                    } else {
                        gauges[i].color = new Color(0.2039f, 0.2039f, 0.2039f);
                    }
                }
                break;
            case StatusType.MoveSpeed:
                icon.sprite = icons[3];
                for (int i = 0; i < gauges.Length; i++) {
                    if (GameManager.Player.Status.Speed >= measure * i) {
                        gauges[i].color = new Color(1, 0.5f, 0.098f);
                    } else {
                        gauges[i].color = new Color(0.2039f, 0.2039f, 0.2039f);
                    }
                }
                break;
            case StatusType.Critical:
                icon.sprite = icons[4];
                for (int i = 0; i < gauges.Length; i++) {
                    if (GameManager.Player.Status.Critical >= measure * i) {
                        gauges[i].color = new Color(1, 0.5f, 0.098f);
                    } else {
                        gauges[i].color = new Color(0.2039f, 0.2039f, 0.2039f);
                    }
                }
                break;
        }
    }
}