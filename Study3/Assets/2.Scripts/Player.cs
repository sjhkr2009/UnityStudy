using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<int, int> EventHpChanged;
    
    
    [SerializeField]
    private int maxHp = 10;
    private int hp = 0;

    public int HP
    {
        get => hp;
        set
        {
            int beforeHp = hp;
            hp = Mathf.Clamp(value, 0, maxHp);
            EventHpChanged(beforeHp, hp);
        }
    }

    private UiHpSlider _uiHpSlider;
    private UiHpText _uiHpText;

    private void Awake()
    {
        hp = maxHp;
    }

    private void Start()
    {
        _uiHpSlider = FindObjectOfType<UiHpSlider>();
        _uiHpText = FindObjectOfType<UiHpText>();

        _uiHpSlider.SetMaxValue(maxHp);
        _uiHpSlider.SetValue(hp);

        _uiHpText.SetText(hp);
    }

    [Button]
    public void HpUp()
    {
        HP += 1;
        _uiHpSlider.SetValue(hp);
        _uiHpText.SetText(hp);
    }

    [Button]
    public void HpDown()
    {
        HP -= 1;
        _uiHpSlider.SetValue(hp);
        _uiHpText.SetText(hp);
    }
}