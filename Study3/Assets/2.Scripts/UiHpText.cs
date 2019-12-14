using UnityEngine;
using UnityEngine.UI;

public class UiHpText : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetText(float value)
    {
        _text.text = $"체력 : {(int)value}";
    }
}