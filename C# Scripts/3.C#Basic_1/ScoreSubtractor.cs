using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSubtractor : MonoBehaviour
{
    //GameManager를 가져와서 사용해본다. (ScoreAdder는 싱글톤 미사용, ScoreSubtractor는 사용)
    //오른쪽 마우스를 누를 때마다 점수를 증가시키는 스크립트.

    //public GameManager gameManager; //해당 오브젝트를 가져오지 않아도 됨.
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.AddScore(-2); //해당 스크립트로부터, 그 스크립트를 가진 오브젝트를 바로 호출한다. (정적 변수이므로 대입하지 않아도 됨.)
            Debug.Log(GameManager.instance.GetScore());
        }
    }
}
