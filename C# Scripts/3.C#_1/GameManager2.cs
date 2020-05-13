using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    //싱글톤 응용
    //이전의 싱글톤(GameManager)은 오브젝트에 스크립트를 넣고, 오브젝트 자신을 정적 변수에 대입한 원시적인 형태인데, 해당 오브젝트가 1개라는 보장이 있어야 사용 가능하다.
    //이를 응용해서, 오브젝트가 없다면 지정해주고, 2개 이상 있다면 1개를 제외하고 파괴하도록 안전장치를 만들 수도 있다.

    public static GameManager2 GetInstance()
    {
        if(instance2 == null) //해당 정적 변수에 어떤 오브젝트도 할당되어있지 않다면,
        {
            instance2 = FindObjectOfType<GameManager2>(); //이 Scene의 모든 오브젝트를 검색해서 GameManager2 컴포넌트를 가진 오브젝트를 찾는다. (단, 비효율적이므로 게임 시작 시 등 적은 횟수로만 사용)

            if(instance2 == null) //그래도 null이라면 (이 Scene의 모든 오브젝트를 찾아봤지만 GameManager2를 가진 오브젝트도 없다면)
            {
                GameObject container = new GameObject("ScoreManager"); //새 게임 오브젝트를 만든다.
                //단, 이 방식으로는 빈 오브젝트만 생성 가능. 특정 형태의 오브젝트 생성은 이전에 다룬 Instantiate를 써야 함.

                instance2 = container.AddComponent<GameManager2>(); //이 스크립트를 컴포넌트로 붙이고, 이 오브젝트를 정적 변수에 대입한다.
                //이렇게 하면 아예 게임상에 GameManager2가 없어도, ScoreAdder2 등에서 이 스크립트를 쓰려는 순간 오브젝트가 생성되며 이 스크립트를 가지게 된다.
                //참고) 사용하려고 할 때 만들어짐 = 지연 생성 방식
            }
        }

        return instance2; //정적 변수를 반환한다. 다른 스크립트에서 정적 변수에 바로 접근하지 않고, 이 함수를 통해 접근하게 한다.
    }

    static GameManager2 instance2; //이 스크립트에 정적 변수 생성. public이 아니므로 바로 접근 불가.

    int score = 0;


    //정적 변수에 대입된 오브젝트가 2개 이상 있을 때, 1개만 남기기
    void Start()
    {
        if(instance2 != null) //정적 변수에 어떤 오브젝트가 들어가 있는데
        {
            if(instance2 != this) //그게 이 오브젝트가 아니라면 (다른 오브젝트가 이미 정적 변수에 대입되어 있다면)
            {
                Destroy(gameObject); //이 오브젝트를 파괴한다.
            }
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }
}
