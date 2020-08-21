using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] GameObject lockImage;
    [SerializeField] Button storyOpenBtn;

    void Start()
    {
        if (title == null)
            title = transform.GetChild(0).GetComponent<Text>();
        if (lockImage == null) 
            lockImage = transform.GetChild(1).gameObject;
        if (storyOpenBtn == null)
            storyOpenBtn = GetComponent<Button>();
    }

    public void SetIcon(int level, bool isLocked)
    {
        SetTitle(level);
        SetImage(isLocked);
        SetButton(level);
    }

    public void SetButton(int level)
    {
        // 추후 스토리 팝업창을 띄우고, 해당 창에서 현재 보고 있는 새의 스토리를 열람하게 처리할 것
        storyOpenBtn.onClick.AddListener(() => { Debug.Log($"{level}번째 스토리 열람"); });
    }

    public void SetTitle(int level)
    {
        if (title == null)
            title = transform.GetChild(0).GetComponent<Text>();

        title.text = $"{level}단계 스토리";
    }

    public void SetImage(bool isLocked)
    {
        if (lockImage == null)
            lockImage = transform.GetChild(1).gameObject;

        lockImage.SetActive(isLocked);
    }
    private void OnDisable()
    {
        storyOpenBtn.onClick.RemoveAllListeners();
    }
}
