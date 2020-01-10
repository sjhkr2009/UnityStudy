using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTest : MonoBehaviour
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
                    if (shootingMove != null)
                    {
                        StopCoroutine(shootingMove);
                    }
                    shootingMove = StartCoroutine(SmoothRotate(mousePos, 1f, false));
                    break;

                case State.Return:
                    _starState = State.Return;
                    shootingMove = StartCoroutine(SmoothReturn());
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

        if (Input.GetMouseButtonDown(0))
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
        rb.velocity = transform.forward * currentMoveSpeed;
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
    }

    void ShootMove()
    {
        
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
        
    }

    Vector3 ReturnPoint()
    {
        Vector3 dir = (transform.position - planet.position);
        dir.Normalize();

        Vector3 dirReverse = new Vector3(-dir.z, dir.y, dir.x).normalized;
        Vector3 returnPoint = (dirReverse * originOrbitalRadius);

        Debug.Log($"현재 위치:{transform.position}, 귀환 지점:{returnPoint}");

        return returnPoint;
    }

    IEnumerator SmoothRotate(Vector3 targetPos, float sensitivity, bool isReturn)
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

            if (Mathf.Abs(angleDifference) < currentRotationSpeed)
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
            else if (angleDifference < 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle - currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
                Debug.Log($"각도 차이: {angleDifference}");
            }

            currentRotationSpeed += rotationAddSpeed;

            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        Debug.Log("코루틴 종료");

        if (!isReturn)
        {
            StarState = State.Return;
        }
        else
        {
            StarState = State.Rotate;
        }
    }

    IEnumerator SmoothReturn()
    {
        currentRotationSpeed = rotationOriginSpeed;

        while (true)
        {
            Vector3 lookPlanetRotation = Quaternion.LookRotation(planet.position - transform.position).eulerAngles;
            Vector3 targetDir = lookPlanetRotation + new Vector3(0, 90f, 0);
            Vector3 currentDir = transform.rotation.eulerAngles;

            float targetAngle = targetDir.y;
            float currentAngle = currentDir.y;

            Debug.Log($"귀환중 / 목표 각도: {targetAngle}, 현재 각도: {currentAngle}");

            float angleDifference = targetAngle - currentAngle;
            if (angleDifference > 180f) angleDifference -= 360f;
            else if (angleDifference < -180f) angleDifference += 360f;

            if (Mathf.Abs(angleDifference) < currentRotationSpeed)
            {
                transform.rotation = Quaternion.Euler(targetDir);
                Debug.Log("복귀 완료");
                break;
            }
            else if (angleDifference > 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle + currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
            }
            else if (angleDifference < 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle - currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
            }

            currentRotationSpeed += rotationAddSpeed;

            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }

        StarState = State.Rotate;
    }
}
