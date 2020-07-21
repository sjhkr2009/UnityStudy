using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class SetCocktailUI : UIBase_Popup
{
    enum Buttons
    {
        NextButton
    }

    enum Texts
    {
        GradeText
    }


    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Idle; });
        GetText((int)Texts.GradeText).text = GradeToText();
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    string GradeToText()
    {
        int grade = GameManager.Data.CurrentGrade;

        switch (grade)
        {
            case -1:
                return "BAD";
            case 0:
                return "Soso";
            case 1:
                return "GOOD!";
            default:
                return "Error";
        }
    }
}
