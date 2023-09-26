using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class RotateMover : MonoBehaviour {
    [TabGroup("Status"), SerializeField] float originOrbitalRadius; //시작 시 공전 궤도 반지름
    [TabGroup("Status"), Range(0f, 1f), SerializeField] float radiusChangeSpeed; //프레임당 목표 궤도에 가까워지는 비율 (mix:0 ~ max:1)
    [TabGroup("Status"), SerializeField] float maxAngulerSpeed; //초당 최대 회전각
    [TabGroup("Status"), SerializeField] float minRadius;
    [TabGroup("Status"), SerializeField] float speedReduction = 1f; //각속도 감소 계수. 높을수록 거리에 따른 이동속도 감소폭이 높다.

    [SerializeField, ReadOnly] float orbitalSpeedFactor = 1f; //각속도 계수. 평상시엔 1이며, 가속 모드일 경우에만 올라간다.

    [SerializeField] private Transform center;

    private float currentAngle;
    private float currentRadius;
    private float targetRadius;

    private float TargetRadius {
        get => targetRadius;
        set => targetRadius = Mathf.Max(minRadius, value);
    }

    private void Awake() {
        TargetRadius = originOrbitalRadius;
    }
    
    private void FixedUpdate() {
        RotateMove();
    }

    void RotateMove() {
        var positionFromCenter = transform.position - center.position;
        
        currentRadius = positionFromCenter.magnitude;
        currentAngle = Mathf.Atan2(positionFromCenter.z, positionFromCenter.x);

        //목표 반지름이 다를 경우 반지름 변화 및 이벤트 발동
        float radius = currentRadius;

        if (Mathf.Abs(TargetRadius - currentRadius) > 0.01f) {
            //목표 반지름과의 차이가 있으면 거리에 따라 이동시킨다
            radius = Mathf.Lerp(currentRadius, TargetRadius, radiusChangeSpeed);
        }
        
        //회전속도에 따라 변화할 각도를, 원래의 반지름으로 돌아가는 속도에 따라 변화할 반지름 길이를 연산
        var angleSpeed = GetAngleSpeed(radius);
        float targetAngle = currentAngle + (angleSpeed * orbitalSpeedFactor) * Mathf.Deg2Rad * Time.fixedDeltaTime;

        //다음 프레임에 이동할 위치 출력
        float nextPosX = radius * Mathf.Cos(targetAngle);
        float nextPosY = radius * Mathf.Sin(targetAngle);
        Vector3 nextPos = center.position + new Vector3(nextPosX, positionFromCenter.y, nextPosY);

        //적용
        transform.LookAt(nextPos);
        transform.position = nextPos;
    }

    float GetAngleSpeed(float radius) {
        return (maxAngulerSpeed * minRadius) / (minRadius + (radius - minRadius) / speedReduction);
    }
}