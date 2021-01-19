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
        NextButton,
        BranchButton1,
        BranchButton2
    }
    enum GameObjects
	{
        BranchButtons
    }

    private int currentIndex = 0;
    private List<string> dialogList = new List<string>();
    Text dialogText;

    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        dialogText = GetText((int)Texts.DialogText);

        if (!SetDialog(GameManager.Dialog.GetDialog(GameManager.Game.CurrentCustomer)))
		{
            GameManager.UI.ClosePopupUI<DialogUI>();
            return;
        }

        Image charImage = GetImage((int)Images.CharacterImage);
        charImage.sprite = GameManager.Game.CurrentCustomer.Image;
        charImage.SetNativeSize();

        Get<GameObject>((int)GameObjects.BranchButtons).SetActive(false);
    }

    public bool SetDialog(List<string> dialog)
	{
        dialogList.Clear();
        for (int i = 0; i < dialog.Count; i++)
            dialogList.Add(dialog[i]);

        currentIndex = 0;

        if (dialogList == null)
        {
            Debug.Log($"대사 탐색 실패 : {GameManager.Game.CurrentCustomer.Name}의 레벨 {GameManager.Game.CurrentCustomer.Level - 1}에 해당하는 대사가 없습니다.");
            return false;
        }

        SetText(currentIndex);

        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.AddListener(SetNextText);
        return true;
    }

    void SetText(int index)
    {
        if (index >= dialogList.Count) return;

        dialogText.DOKill();

        string text = CheckString(dialogList[index]);
        dialogText.text = "";
        dialogText.DOText(text, text.Length * Define.DoTextSpeedFast);

    }
    void SetNextText()
    {
        currentIndex++;
        if (currentIndex == dialogList.Count - 1)
            NextButtonToClose();

        if (currentIndex >= dialogList.Count)
            return;

        SetText(currentIndex);
    }

    string CheckString(string str)
	{
        if (!str.Contains(Define.BranchCommend))
            return str;

        int index = str.IndexOf(Define.BranchCommend);
        string[] command = str.Substring(index + 2).Split(Define.SplitCommend);
        string text = str.Substring(0, index);

        if (command[0] == Define.BrCmd_Dialog)
            BranchDialog(int.Parse(command[1]), int.Parse(command[2]));


        return text;
    }

    void BranchDialog(int cmd1, int cmd2)
	{
        Get<GameObject>((int)GameObjects.BranchButtons).SetActive(true);

        GetButton((int)Buttons.BranchButton1).onClick.RemoveAllListeners();
        GetButton((int)Buttons.BranchButton2).onClick.RemoveAllListeners();

        GetButton((int)Buttons.BranchButton1).onClick.AddListener(() =>
        {
            SetDialog(GameManager.Dialog.GetBranchDialog(cmd1));
            Get<GameObject>((int)GameObjects.BranchButtons).SetActive(false);
        });
        GetButton((int)Buttons.BranchButton2).onClick.AddListener(() =>
        {
            SetDialog(GameManager.Dialog.GetBranchDialog(cmd2));
            Get<GameObject>((int)GameObjects.BranchButtons).SetActive(false);
        });
    }

    void NextButtonToClose()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => 
        {
            dialogText.DOKill();
            GameManager.Instance.GameState = GameState.Idle;
        });
    }
}
