using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{
    Image fillImage;

    void Start()
    {
        if (fillImage == null) 
            fillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetIcon(float posRate, bool isFill)
    {
        SetTransform(posRate);
        SetFill(isFill);
    }

    public void SetTransform(float posRate)
    {
        transform.position = new Vector2(transform.position.x, Define.BirdLevelSliderWidth * posRate);
    }
    public void SetFill(bool isFill)
    {
        if (fillImage == null)
            fillImage = transform.GetChild(0).GetComponent<Image>();

        fillImage.fillAmount = isFill ? 1f : 0f;
    }
    
}
