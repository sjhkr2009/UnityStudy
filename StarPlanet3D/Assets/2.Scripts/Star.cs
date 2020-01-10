using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public enum State { Rotate, Shoot, Return }


    //공전
    [SerializeField] float originOrbitalRadius; //원래의 공전 궤도 반지름
    [SerializeField] float radiusChangeSpeed; //프레임당 원래의 궤도에 가까워지는 비율 (mix:0 ~ max:1)
    [SerializeField] float angulerSpeed; //초당 회전각
    private float currentAngle;
    private float currentRadius;

    //사출
    [SerializeField] private float originMoveSpeed;
    [SerializeField] private float rotationOriginSpeed;
    [SerializeField] private float rotationAddSpeed;
    private float currentMoveSpeed;

    //외부 컴포넌트
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform planet;
    public AnimationCurve m_distanceInfluence = AnimationCurve.Linear(0, 1, 1, 1);

    private Vector3 mousePos; // => Controller 스크립트에서 처리할 것

    private Coroutine shootingMove;
    private float currentRotationSpeed;

    private State _starState;
    public State StarState
    {
        get => _starState;
        set
        {
            switch (value)
            {
                case State.Rotate:
                    _starState = State.Rotate;
                    break;

                case State.Shoot:
                    _starState = State.Shoot;
                    if(shootingMove != null)
                    {
                        StopCoroutine(shootingMove);
                    }
                    shootingMove = StartCoroutine(SmoothRotate(mousePos, 1f));
                    break;

                case State.Return:
                    _starState = State.Return;
                    shootingMove = StartCoroutine(SmoothRotate(ReturnPoint(), 0.85f));
                    break;
            }
        }
    }

    void Start()
    {
        StarState = State.Rotate;
        currentRotationSpeed = rotationOriginSpeed;
        currentMoveSpeed = originMoveSpeed;
    }

    
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        if (Input.GetMouseButton(0))
        {
            Debug.Log("발사");
            StarState = State.Shoot;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        switch (StarState)
        {
            case State.Rotate:
                RotateMove();
                break;
            case State.Shoot:
                ShootMove();
                break;
            case State.Return:
                ReturnMove();
                break;
        }
    }

    void RotateMove()
    {
        //현재 중심부로부터의 길이와 각도 구하기
        currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        //회전속도에 따라 변화할 각도를, 원래의 반지름으로 돌아가는 속도에 따라 변화할 반지름 길이를 연산
        float _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float _targetRadius = Mathf.Lerp(currentRadius, originOrbitalRadius, radiusChangeSpeed);

        //다음 프레임에 이동할 위치 출력
        float _nextPosX = _targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = _targetRadius * Mathf.Sin(_targetAngle);
        Vector3 _nextPos = new Vector3(_nextPosX, transform.position.y, _nextPosY);

        //방향 조정
        transform.LookAt(_nextPos);

        //속도 기록
        //currentMoveSpeed = Vector3.Distance(transform.position, _nextPos) / Time.deltaTime;

        //적용
        //transform.position = _nextPos;
        rb.velocity = transform.forward * currentMoveSpeed;
    }

    void ShootMove()
    {
        transform.Translate(Time.deltaTime * Vector3.forward * currentMoveSpeed);
    }

    void TestGoToTarget(Vector3 targetPoint)
    {
        Vector3 m_forward = rb.velocity;
        Vector3 m_direction = (targetPoint - transform.position).normalized * m_distanceInfluence.Evaluate(1 - (targetPoint - transform.position).magnitude / 10);
        rb.velocity = rb.velocity * 0.9f + m_direction;
        Debug.Log(rb.velocity);

        m_forward = rb.velocity.normalized;
        transform.LookAt(rb.velocity);
    }

    void ReturnMove()
    {
        currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        float _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float _targetRadius = Mathf.Lerp(currentRadius, originOrbitalRadius, radiusChangeSpeed);

        float _nextPosX = _targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = _targetRadius * Mathf.Sin(_targetAngle);
        Vector3 _nextPos = new Vector3(_nextPosX, transform.position.y, _nextPosY);

        
        transform.LookAt(_nextPos);

        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, originMoveSpeed, 0.05f);
        transform.Translate(Time.deltaTime * Vector3.forward * currentMoveSpeed);

        if (Mathf.Abs(_targetRadius - originOrbitalRadius) < 1)
        {
            StarState = State.Rotate;
        }
    }

    Vector3 ReturnPoint()
    {
        Vector3 dir = (transform.position - planet.position);
        dir.Normalize();

        Vector3 dirReverse = new Vector3(-dir.z, dir.y, dir.x).normalized;
        Vector3 returnPoint = (dirReverse * originOrbitalRadius);

        return returnPoint;
    }

    IEnumerator SmoothRotate(Vector3 targetPos, float sensitivity)
    {
        currentRotationSpeed = rotationOriginSpeed;
        float _distanceToTarget = Vector3.Distance(transform.position, targetPos);
        Vector3 startPosition = transform.position;
        float _movedDistance = 0f;
        float _currentDistanceToTarget = _distanceToTarget;

        while (_movedDistance < _distanceToTarget * sensitivity || _currentDistanceToTarget > 2f)
        {
            _currentDistanceToTarget = Vector3.Distance(transform.position, targetPos);
            Debug.Log($"작동중 / 이동한 거리: {_movedDistance}, 이동할 거리: {_distanceToTarget}, 목표까지의 거리: {_currentDistanceToTarget}");
            _movedDistance = Vector3.Distance(transform.position, startPosition);

            Vector3 targetDir = Quaternion.LookRotation(targetPos - transform.position).eulerAngles;
            Vector3 currentDir = transform.rotation.eulerAngles;
            float targetAngle = targetDir.y;
            float currentAngle = currentDir.y;

            float angleDifference = targetAngle - currentAngle;
            if (angleDifference > 180f) angleDifference -= 360f;
            else if (angleDifference < -180f) angleDifference += 360f;

            if(Mathf.Abs(angleDifference) < currentRotationSpeed)
            {
                transform.rotation = Quaternion.Euler(targetDir);
                Debug.Log("조준 완료");
            }
            else if (angleDifference > 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle + currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
                Debug.Log($"각도 차이: {angleDifference}");
            } 
            else if(angleDifference < 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle - currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
                Debug.Log($"각도 차이: {angleDifference}");
            }

            currentRotationSpeed += rotationAddSpeed;
            //임시 코드
            currentMoveSpeed = currentMoveSpeed * 0.75f + _currentDistanceToTarget * 0.3f;

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("코루틴 종료");
        StarState = State.Return;
    }
}
