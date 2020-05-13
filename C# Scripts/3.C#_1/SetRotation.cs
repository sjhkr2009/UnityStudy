using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation : MonoBehaviour
{
    public Transform targetTransform; //바라볼 대상을 가져온다 (50번째 줄에서 사용)


    //회전과 쿼터니언

    //유니티의 트랜스폼은 Rotation에 벡터 값을 넣어 회전하는 듯 보이지만, 이는 편의상의 표기.
    //실제로 회전은 Quaternion을 통해 조정된다.
    void Start()
    {
        //transform.rotation = new Vector3(60, 60, 60); //벡터 값을 입력해도 변환할 수 없다고 뜬다.
        //rotation에 마우스를 올려보면 Quaternion이 사용됨을 확인할 수 있다.


        //오일러 방식: x,y,z축에 값을 입력하면, x,y,z축을 기준으로 물체를 회전시키는 방식. 이를 이용해 유니티는 벡터로 회전을 표현한다.

        //오일러 방식의 문제점
        //그런데 x,y,z축 동시에 회전하는 게 아니라 순차적으로 진행되는데, 어느 축이 정확히 90도 회전하면 나머지 축의 정보가 겹치는 현상이 생긴다.
        //즉, x축이 90도 회전 시 회전 전후의 y,z 축이 완전히 동일해진다. 이 때 y,z축 중이 같아서 어느 것을 기준으로 회전해야 할 지 이해할 수 없다. - 짐벌 락(gimbal lock) 현상
        //결국 두 축이 하나로 인식된다. 즉 두 축 중 하나의 정보가 소실되어, y축 기준 회전과 z축 기준 회전이 똑같은 결과를 보여준다. (이를 피하기 위해 고전 게임은 90.1도, 89.9도 회전시키기도 했다)


        //쿼터니언은 복잡한 수를 기반으로 하고, 직관적으로 이해하기 쉽지 않다. - by 유니티 도움말
        //유니티에서도 쿼터니언 함수를 직접 조작하는 것은 막아 놓았으며, Vector3 를 이용해 쿼터니언을 조작하도록 만들어져 있다.
        //따라서 우리는 미리 정해진 쿼터니언 함수를 사용하면 된다.
        
    }


    void Update()
    {
        //Quaternion.Euler() : 벡터 값을 통해 회전을 표현시켜준다. 괄호 안에는 벡터 값을 입력한다.

        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion newRotation = Quaternion.Euler(new Vector3(30, 30, 30)); //벡터를 통해 쿼터니언 함수를 조작하여, 30도씩 회전시키는 값을 생성
            transform.rotation = newRotation; //rotation에 적용
        }

        //Quaternion.LookRotation() : 물체가 해당 방향을 바라보도록 설정한다. 괄호 안에는 바라볼 지점의 좌표를 입력한다. (참고로 물체의 Z방향이 앞쪽이므로, 이걸 기준으로 바라보게 됨)
        //참고) 상대방 위치(Vector3)에서 내 위치를 빼면 내가 얼마나 가야 상대방 위치가 되는지 구할 수 있다.

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = targetTransform.position - transform.position; //상대 위치에서 내 위치를 빼면, 상대방까지의 거리와 방향이 나온다.
            Quaternion targetRotation = Quaternion.LookRotation(direction); //해당 방향을 바라보는 각도로 회전값을 생성
            transform.rotation = targetRotation; //적용
        }

        //Quaternion.Lerp(a,b,x) : 두 지점 사이 어딘가를 바라보게 한다. a,b에는 두 지점의 회전값(Vector3를 입력 후 Euler로 변환)을, x에는 어느 지점을 바라볼 지를 0~1 사이의 숫자(float)로 나타낸다.
        //예를 들어, x자리에 0.5f를 입력 시 정확히 두 지점의 중간지점을 향한 회전값이 생성된다.

        if (Input.GetKey(KeyCode.E))
        {
            Quaternion aRotation = Quaternion.Euler(new Vector3(30, 0, 0)); //A,B에 각각 X축 기준 30도, 60도라는 회전값을 입력)
            Quaternion bRotation = Quaternion.Euler(new Vector3(60, 0, 0));

            Quaternion middleRotation = Quaternion.Lerp(aRotation, bRotation, 0.5f); //중간 지점을 보게 하면 X축 기준 45도 회전값이 생성된다. 만약 0.2f를 입력했다면 36도 회전일 것이다.
            transform.rotation = middleRotation; //적용
        }

        //transform.Rotate(): 현재 회전에서 지정한 만큼 더 회전한다. 괄호에는 Vector3 값을 그대로 넣으면 된다.

        if (Input.GetKeyDown(KeyCode.A)) //특정 값으로 바꾸는 게 아니라 계속 회전시키므로, 여기선 누를 때 한 번만 실행되는 GetKeyDown으로 실험한다.
        {
            transform.Rotate(new Vector3(30, 0, 0));
        }

        //특정 물체 혹은 자신의 회전값에서 정해진 수치만큼 더 회전한다.
        //대상의 회전값을 쿼터니언으로 가져와서 벡터3로 변환하고, 벡터3끼리 연산한 다음, 다시 쿼터니언으로 변환시켜 적용해야 한다.
        //이 때 회전값.eulerAngles 를 통해 쿼터니언을 벡터로 변환할 수 있다.

        if (Input.GetKeyDown(KeyCode.S))
        {
            Quaternion originalRotation = transform.rotation; //현재 회전값을 저장하기 휘한 변수
            Vector3 originalRotationInVector3 = originalRotation.eulerAngles; //쿼터니언으로 표시괸 현재 회전값을 벡터3 값으로 변경한 변수.
            Debug.Log(originalRotationInVector3); //(확인용)

            Vector3 targetInVector3 = originalRotationInVector3 + new Vector3(0, 30, 0); //벡터3로 변환한 값에서 원하는 만큼 추가 회전
            Quaternion target = Quaternion.Euler(targetInVector3); //쿼터니언으로 변환

            transform.rotation = target; //적용
        }

        //위 과정을 좀 더 단순화해서, 벡터로 변환하지 않고 쿼터니언끼리 계산할수도 있다.
        //쿼터니언끼리 더하고 싶을 때는 서로 곱하면 된다.
        if (Input.GetKeyDown(KeyCode.D))
        {
            //더하고 싶은 두 개의 회전값을 각각 쿼터니언으로 만들고,
            Quaternion originalRot = Quaternion.Euler(new Vector3(45, 0, 0));  
            Quaternion plusRot = Quaternion.Euler(new Vector3(25, 0, 0));

            //두 값을 곱한다
            Quaternion resultRot = originalRot * plusRot; //이 경우 회전값은 (70,0,0)을 쿼터니언으로 변환한 값이 된다.

            transform.rotation = resultRot; //적용
        }

        //주의사항: 회전 역시 transform이므로, 자신의 상대적인 x,y,z축을 기준으로 회전한다.

    }
}
