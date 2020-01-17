using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Star : Player
{
    public event Action<Star> EventPlayerDead;

    [SerializeField] float originOrbitalRadius; //원래의 공전 궤도 반지름
    [SerializeField] float radiusChangeSpeed; //프레임당 목표 궤도에 가까워지는 비율 (mix:0 ~ max:1)
    [SerializeField] float originAngulerSpeed; //초당 최대 회전각
    [SerializeField] float minRadius;
    [SerializeField] float speedReduction = 1f;
    private float currentAngle;
    private float currentRadius;
    private float _targetRadius;
    private float angulerSpeed;

    private float targetRadius
    {
        get => _targetRadius;
        set
        {
            _targetRadius = Mathf.Max(minRadius, value);
        }
    }

    public event Action<float> EventRadiusChange;

    private void Awake()
    {
        targetRadius = originOrbitalRadius;
        EventRadiusChange += AngularSpeedChange;
    }

    private void OnDestroy()
    {
        EventRadiusChange -= AngularSpeedChange;
    }

    protected override void Start()
    {
        base.Start();
        EventRadiusChange(Vector3.Distance(transform.position, Vector3.zero));

    }

    public override void Processing()
    {
        RotateMove();
    }

    void RotateMove()
    {
        currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        //회전속도에 따라 변화할 각도를, 원래의 반지름으로 돌아가는 속도에 따라 변화할 반지름 길이를 연산
        float _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;

        //목표 반지름이 다를 경우 반지름 변화 및 이벤트 발동
        float _targetRadius = currentRadius;
        if (targetRadius != currentRadius && Mathf.Abs(targetRadius - currentRadius) < 0.01f)
        {
            _targetRadius = targetRadius;
            EventRadiusChange(_targetRadius);
        }
        else if (targetRadius != currentRadius && Mathf.Abs(targetRadius - currentRadius) >= 0.01f)
        {
            _targetRadius = Mathf.Lerp(currentRadius, targetRadius, radiusChangeSpeed);
            EventRadiusChange(_targetRadius);
        }

        //다음 프레임에 이동할 위치 출력
        float _nextPosX = _targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = _targetRadius * Mathf.Sin(_targetAngle);
        Vector3 _nextPos = new Vector3(_nextPosX, transform.position.y, _nextPosY);

        //적용
        transform.position = _nextPos;
    }

    public void TargetRadiusChange(Vector3 mousePos)
    {
        targetRadius = Vector3.Distance(mousePos, Vector3.zero);
    }

    void AngularSpeedChange(float radius)
    {
        angulerSpeed = originAngulerSpeed * minRadius / (minRadius + (radius - minRadius) / speedReduction);
    }

    private void OnDisable()
    {
        EventPlayerDead(this);
    }
}
