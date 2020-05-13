using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTest2 : MonoBehaviour
{
    [SerializeField] float originOrbitalRadius; //원래의 공전 궤도 반지름
    [SerializeField] float radiusChangeSpeed; //프레임당 목표 궤도에 가까워지는 비율 (mix:0 ~ max:1)
    [SerializeField] float angulerSpeed; //초당 회전각
    [SerializeField] GameObject radiusUI;
    private float currentAngle;
    private float currentRadius;
    private float targetRadius;

    void Start()
    {
        targetRadius = originOrbitalRadius;
        radiusUI.transform.localScale = Vector3.one * targetRadius * 2;
        currentRadius = Vector3.Distance(transform.position, Vector3.zero);
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0)) //나중에 GameManager에서 RadiusChange(Vector3 targetPos)를 실행하게 할 것. 동시에 UI역할을 하는 Sprite의 크기를 변경하기.
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            targetRadius = Vector3.Distance(mousePos, Vector3.zero);
        }
    }

    private void FixedUpdate()
    {
        RotateMove();
    }

    void RotateMove()
    {
        currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        //회전속도에 따라 변화할 각도를, 원래의 반지름으로 돌아가는 속도에 따라 변화할 반지름 길이를 연산
        float _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float _targetRadius = Mathf.Lerp(currentRadius, targetRadius, radiusChangeSpeed);

        //다음 프레임에 이동할 위치 출력
        float _nextPosX = _targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = _targetRadius * Mathf.Sin(_targetAngle);
        Vector3 _nextPos = new Vector3(_nextPosX, transform.position.y, _nextPosY);

        //UI조정(임시)
        radiusUI.transform.localScale = Vector3.one * currentRadius * 2;

        //적용
        transform.position = _nextPos;
    }
}
