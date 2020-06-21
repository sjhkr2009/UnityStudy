using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G01_ButtonTest : MonoBehaviour
{
    // 클라이언트 개발에서 대부분의 시간은 UI의 연결에 사용한다.

    // 인스펙터에서 Button의 OnClick 함수에 특정 함수를 연결하면 버튼 클릭 시 실행된다. 툴로 구현한 옵저버 패턴과 유사함.
    // public으로 선언한 함수만 연결할 수 있다.

    [SerializeField] Text text;
    int score = 0;
    public void OnButtonClicked()
    {
        score++;
        Debug.Log("버튼 클릭됨");
        text.text = $"점수 : {score}";
    }

    // UI를 클릭해도 뒤의 게임오브젝트가 클릭되는 문제는, EventSystem.current.IsPointerOverGameObject() 를 통해 UI이벤트의 발동여부를 체크하여 추가 동작을 무시할 수 있다.
    // 이는 B02_InputManager에서 구현함.

    // 단, OnClick 등으로 툴에서 구현하는 것은 소규모 클리커 게임에선 가능하겠지만, 게임 규모가 커지면 이런 방식으로 버튼 관련 변수와 함수를 만들어 작성하는 것은 권장되지 않는다.
    // 연결 과정에서 실수가 일어나거나 유지보수도 어렵기 때문에, 툴 상에서 드래그 앤 드롭을 하기보다 코드로 연결시키는 것이 좋음.
    // (예를 들어 리그 오브 레전드의 플레이 화면에만 수백 가지의 UI 관련 동작이 들어 있다.)
}
