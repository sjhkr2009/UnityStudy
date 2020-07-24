using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUI : UIBase_Popup
{
    enum Images
    {
        BackgroundImage,
        CharacterImage,
        TableImage,
        NextButton
    }
    enum Texts
    {
        DialogText,
        ButtonText
    }
    enum Buttons
    {
        NextButton
    }

    private int currentIndex = 0;
    private List<string> dialogList = new List<string>();
    [SerializeField] Sprite endButtonImage;

    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        currentIndex = 0;
        dialogList = DataManager.DialogData.GetDialog(GameManager.Data.CurrentCustomer);

        SetText(currentIndex);
        GetButton((int)Buttons.NextButton).onClick.AddListener(SetNextText);

        Image charImage = GetImage((int)Images.CharacterImage);
        charImage.sprite = GameManager.Data.CurrentCustomer.image;
        charImage.SetNativeSize();
    }

    void SetText(int index)
    {
        if (index >= dialogList.Count) return;

        string text = dialogList[index];
        GetText((int)Texts.DialogText).text = "";
        GetText((int)Texts.DialogText).DOText(text, text.Length * Define.DoTextSpeedFast);

    }
    void SetNextText()
    {
        currentIndex++;
        if (currentIndex == dialogList.Count - 1) NextButtonToCancel();

        if (currentIndex >= dialogList.Count) return;
        SetText(currentIndex);
    }
    void NextButtonToCancel()
    {
        if(endButtonImage == null)
            GetText((int)Texts.ButtonText).text = "닫기";
        else
            GetImage((int)Images.NextButton).sprite = endButtonImage;

        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<DialogUI>(); });
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
}
