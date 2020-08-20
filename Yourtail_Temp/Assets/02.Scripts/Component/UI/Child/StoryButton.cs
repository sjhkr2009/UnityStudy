using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour
{
    Text title;
    GameObject lockImage;
    void Start()
    {
        if (title == null)
            title = transform.GetChild(0).GetComponent<Text>();
        if (lockImage == null) 
            lockImage = transform.GetChild(1).gameObject;
    }

    public void SetIcon(int index, bool isLocked)
    {
        SetTitle(index);
        SetImage(isLocked);
    }

    public void SetTitle(int index)
    {
        if (title == null)
            title = transform.GetChild(0).GetComponent<Text>();

        title.text = $"{index}단계 스토리";
    }

    public void SetImage(bool isLocked)
    {
        if (lockImage == null)
            lockImage = transform.GetChild(1).gameObject;

        lockImage.SetActive(isLocked);
    }
}
