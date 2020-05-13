using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI에 접근하는 함수를 사용하기 위해 불러온다. (Image 등)

public class FadeOut_Good : MonoBehaviour
{
    //Fade In, Fade Out 구현 - 코루틴 사용
    //코루틴의 첫 번째 기능: 대기시간을 삽입한다.

        //참고) 코루틴은 C# 고유의 문법이 아닌 유니티의 기능으로, 문법에 크게 신경쓸 필요는 없음.

    public Image fadeImage; //이 오브젝트가 가진 Image 컴포넌트를 가져온다. (R,G,B 및 알파값 조정 가능)

    void Start()
    {
        StartCoroutine(FadeIn()); //조금 더 성능이 좋지만 함수 형식으로 발동되므로 도중에 멈출 수 없음.
        //StartCoroutine("FadeIn"); //수동으로 종료시킬 수 있음.
        //이 둘의 구분은 HelloCoroutine 스크립트를 참고.
    }

    IEnumerator FadeIn()
    {
        Color startColor = fadeImage.color;

        for (int i = 0; i < 100; i++) //0부터 100까지 반복한다.
        {
            startColor.a -= 0.01f; //1회 반복마다 투명도를 1%씩 뺀다.
            fadeImage.color = startColor; //뺀 값을 실제 색상에 대입한다.
            yield return new WaitForSeconds(0.01f); //1회 반복 사이에 0.01초씩 대기한다. (yield문 만나면 정해진 만큼 쉰 다음 계속 스크립트를 진행)

            //이렇게 하면 100회 반복을 1초에 걸쳐 할 수 있다.
        }
    }

    void Update()
    {

    }
}
