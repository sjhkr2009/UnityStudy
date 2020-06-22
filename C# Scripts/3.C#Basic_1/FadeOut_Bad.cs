using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI에 접근하는 함수를 사용하기 위해 불러온다.

public class FadeOut_Bad : MonoBehaviour
{
    //Fade In, Fade Out 구현 - 코루틴 미사용 시

    public Image fadeImage; //이 오브젝트가 가진 Image 컴포넌트를 가져온다. (R,G,B 및 알파값 조정 가능)

    void Start()
    {
        FadeIn();
    }
    
    void FadeIn()
    {
        Color startColor = fadeImage.color; //시작 컬러를 Color 변수로 정의해주고, 현재 이 이미지의 색상을 대입한다.
        //Color 변수는 r,g,b,a(투명도)의 값을 가진다. 조정할 때는 startColor.r 와 같은 식으로 나타낸다.
        //원래 각각 0~255 사이의 값을 가지지만, 스크립트상에선 퍼센트로 표시되므로 0~1 사이의 float값을 가진다. (1.0f = 255)

        for(int i=0; i<100; i++) //0부터 100까지 반복한다.
        {
            startColor.a -= 0.01f; //1회 반복마다 투명도를 1%씩 뺀다.
            fadeImage.color = startColor; //뺀 값을 실제 색상에 대입한다.
        }

        //이렇게 코드를 작성하면, 컴퓨터는 위 100번의 연산을 순식간에 할 것이고, 오브젝트는 순식간에 투명해질 것이다.
        // -> 즉, 반복과 반복 사이의 대기 시간이 필요하다. 이렇게 대기 시간이 필요할 때 코루틴을 사용한다.
    }

    void Update()
    {
        
    }
}
