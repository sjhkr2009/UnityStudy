using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{
    [SerializeField] RectTransform tr;
    [SerializeField] Image fillImage;

    void Start()
    {
        ComponentCheck();
    }
    void ComponentCheck()
    {
        if (fillImage == null)
            fillImage = transform.GetChild(0).GetComponent<Image>();
        if (tr == null)
            tr = GetComponent<RectTransform>();
    }

    public void SetIcon(float posRate, bool isFill)
    {
        SetTransform(posRate);
        SetFill(isFill);
    }

    public void SetTransform(float posRate)
    {
        if (tr == null)
            tr = GetComponent<RectTransform>();

        tr.anchorMin = new Vector2(posRate, 0f);
        tr.anchorMax = new Vector2(posRate, 0f);
    }
    public void SetFill(bool isFill)
    {
        if (fillImage == null)
            fillImage = transform.GetChild(0).GetComponent<Image>();

        fillImage.fillAmount = isFill ? 1f : 0f;
    }
    
}
