using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    //GameManager를 가져와서 사용해본다. (ScoreAdder는 싱글톤 미사용, ScoreSubtractor는 사용)
    //왼쪽 마우스를 누를 때마다 점수를 증가시키는 스크립트.

    public GameManager gameManager; //이 스크립트를 가진 오브젝트가 들어갈 공간을 만든다. 이후 유니티에서 드래그 앤 드롭으로 해당 오브젝트를 넣어준다.
    //이렇게 하면 점수와 연관이 있는 오브젝트마다 GameManager를 가져와야 한다.

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameManager.AddScore(5);
            Debug.Log(gameManager.GetScore());
        }
    }
}
