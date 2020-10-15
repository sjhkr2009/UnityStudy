using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdInfoWindow : UIBase_Popup
{
    enum Images
    {
        BirdImage
    }

    enum HeartIcons
    {
        HeartIcon1,
        HeartIcon2,
        HeartIcon3,
        HeartIcon4,
        HeartIcon5,
        HeartIcon6,
        Count
    }

    enum StoryButtons
    {
        StoryButton1,
        StoryButton2,
        StoryButton3,
        StoryButton4,
        StoryButton5,
        StoryButton6,
        Count
    }

    enum Buttons
	{
        CloseButton,
        blockRaycast
    }

    enum Sliders
    {
        LevelBar
    }

    enum Texts
    {
        LikingText
    }

    enum RectTransforms
    {
        Background,
        StoryList
    }


    Customers myBird;
    
    void Start() => Init();
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<HeartIcon>(typeof(HeartIcons));
        Bind<StoryButton>(typeof(StoryButtons));
        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<BirdInfoWindow>(); });
        GetButton((int)Buttons.blockRaycast).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<BirdInfoWindow>(); });

        // + 스토리 팝업창 띄우는 동작 추가 필요 -> StoryButton에서 관리

        SetInfo();

        SetPooling();
    }

    void OnEnable()
    {
        if (!Inited || !DontDestroy)
            return;

        gameObject.SetCanvasOrder();
        SetInfo();
    }

    void SetInfo()
    {
        myBird = GameManager.UI.CurrentBirdInfo;
        if (myBird == null)
        {
            Debug.Log("UI Manager에서 표기할 새의 정보가 확인되지 않습니다.");
            GameManager.UI.ClosePopupUI<BirdInfoWindow>();
            return;
        }

        SetBasicInfo();
        SetObjects();
        SetLevelSlider();
    }

    void SetBasicInfo()
    {
        GetImage((int)Images.BirdImage).sprite = myBird.Image;
        GetText((int)Texts.LikingText).text = myBird.Liking;
    }
    void SetObjects()
    {
        for (int i = 0; i < (int)HeartIcons.Count; i++)
        {
            HeartIcon icon = Get<HeartIcon>(i);
            StoryButton story = Get<StoryButton>(i);
            if (i >= myBird.MaxLevel)
            {
                icon.gameObject.SetActive(false);
                story.gameObject.SetActive(false);
            }
            else
            {
                icon.gameObject.SetActive(true);
                story.gameObject.SetActive(true);
                icon.SetIcon((float)i / (myBird.MaxLevel - 1), (myBird.Level > i));
                story.SetIcon(i + 1, (myBird.Level <= i + 1));
            }
        }
        Get<RectTransform>((int)RectTransforms.StoryList).sizeDelta = new Vector2(0, (myBird.MaxLevel * Define.StoryButtonsSpacing) + 25);
    }
    void SetLevelSlider()
    {
        Slider slider = Get<Slider>((int)Sliders.LevelBar);
        slider.value = 0f;

        int spaceCount = myBird.MaxLevel - 1;
        float sliderValue = ((float)(myBird.Level - 1) / spaceCount) + ((1f / spaceCount) * ((float)myBird.Exp / Define.RequiredEXP[myBird.Level]));

        slider.DOValue(sliderValue, 0.5f).SetDelay(0.2f);
    }

    private void OnDestroy()
    {
        Get<Slider>((int)Sliders.LevelBar).DOKill();
        GetButton((int)Buttons.CloseButton).onClick.RemoveAllListeners();
    }
    private void OnDisable()
    {
        Get<RectTransform>((int)RectTransforms.Background).localScale = Vector3.one;
    }
}
