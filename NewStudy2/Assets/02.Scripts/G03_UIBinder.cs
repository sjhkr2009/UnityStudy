using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class G03_UIBinder : G08_UIPopup
{
    // 각 캔버스 프리팹마다 붙는 스크립트로, 이 캔버스 산하에 있는 UI들의 동작을 제어한다.
    // 프리팹 이름과 스크립트명을 동일하게 만드는 것이 권장된다.


    // enum에는 산하의 오브젝트명을 넣어 매핑한다. 전부 GameObject로 할 수도 있고, GameObject와 함께 자주 쓰이는 Text, Image, Button 타입을 따로 정리해도 된다.
    public enum Texts
    {
        ScoreText,
        ButtonText
    }

    public enum Buttons
    {
        ScoreButton
    }



    // 제어할 내부 변수. 인스펙터창에서 설정할 필요는 없이 코드상으로 제어할 것이니 private으로 선언한다.

    int score;
    Button scoreButton;
    Text scoreText;

    private void Start()
    {
        Init();
    }

    // UI 종류에 따라 씬 또는 팝업을 상속받고, 그곳에서 virtual Init()을 만들어 캔버스를 설정한다.
    // 여기서는 기본적인 캔버스 설정에 더해서, 이 캔버스에서 설정할 것들을 넣는다.
    protected override void Init()
    {
        base.Init();

        // enum으로 정리한 산하의 오브젝트들을 딕셔너리로 저장한다. 가져오고 싶은 컴포넌트들을 enum을 통해 Get 함수로 불러올 수 있다.
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        // Get 함수에 인자로 enum값을 넣어 컴포넌트를 불러온다.
        score = 0;
        scoreButton = GetButton((int)Buttons.ScoreButton);
        scoreText = GetText((int)Texts.ScoreText);

        // 컴포넌트가 클릭, 드래그 등 사용자의 입력을 받았을 때의 이벤트도 여기서 설정한다. 인스펙터 창의 드래그 앤 드롭을 대신한다.
        // 원시적인 방법으로는 아래와 같이 이벤트를 추가해줄 수 있다.
        //scoreText.gameObject.AddComponent<G06_EventHandler>().EventOnDrag += ScoreTextOnDrag;
        //scoreButton.gameObject.AddComponent<G06_EventHandler>().EventOnClick += ScoreButtonOnClick;

        //이를 확장 메서드를 이용해 변수 없이 좀더 간편하게 표현하면 다음과 같다.
        GetButton((int)Buttons.ScoreButton).gameObject.AddUIEvent(ScoreButtonOnClick);
        GetText((int)Texts.ScoreText).gameObject.AddUIEvent(ScoreTextOnDrag, E02_Define.EventType.Drag);

        // 초기 값 설정이 필요한 경우 지정한다. 
        // 단, 오브젝트 파괴 후 재생성 시 프리팹에 저장된 값이 다시 호출되니, 게임 내에서 변경한 값의 저장이 필요하면 SetActive(true/false)를 사용하자.
        GetText((int)Texts.ButtonText).text = "점수 증가";
        scoreText.text = $"점수 : {score}";
    }

    // 이벤트에 추가해 줄 함수들. PointerEventData를 매개변수로 사용한다. using UnityEngine.EventSystems이 필요하다.
    void ScoreTextOnDrag(PointerEventData evt)
    {
        scoreText.transform.position = evt.position;
    }
    void ScoreButtonOnClick(PointerEventData evt)
    {
        score++;
        Debug.Log("버튼 클릭됨");
        scoreText.text = $"점수 : {score}";
    }
}
