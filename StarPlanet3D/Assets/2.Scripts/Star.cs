using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public enum State { Rotate, Shoot, Return }
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationOriginSpeed;
    [SerializeField] private float rotationAddSpeed;

    [SerializeField] Rigidbody rb;

    private Vector3 mousePos; // => Controller

    private Coroutine currentMove;
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
                    if(currentMove != null)
                    {
                        StopCoroutine(currentMove);
                    }
                    currentMove = StartCoroutine(SmoothRotate(mousePos));
                    break;

                case State.Return:
                    _starState = State.Return;
                    break;
            }
        }
    }

    void Start()
    {
        StarState = State.Rotate;
        currentRotationSpeed = rotationOriginSpeed;
    }

    
    void Update()
    {
        Move();
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        if (Input.GetMouseButtonDown(0))
        {
            StarState = State.Shoot;
        }
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

    }

    void ShootMove()
    {
        transform.Translate(Time.deltaTime * Vector3.forward * moveSpeed);
    }

    void ReturnMove()
    {

    }

    IEnumerator SmoothRotate(Vector3 targetPos)
    {
        currentRotationSpeed = rotationOriginSpeed;

        while (true)
        {
            Vector3 targetDir = Quaternion.LookRotation(targetPos - transform.position).eulerAngles;
            Vector3 currentDir = transform.rotation.eulerAngles;
            float targetAngle = targetDir.y;
            float currentAngle = currentDir.y;

            float angleDifference = targetAngle - currentAngle;
            if (angleDifference > 180f) angleDifference -= 360f;
            else if (angleDifference < -180f) angleDifference += 360f;

            Debug.Log($"각도 차이: {angleDifference}");

            if(Mathf.Abs(angleDifference) < currentRotationSpeed)
            {
                transform.rotation = Quaternion.Euler(targetDir);
                Debug.Log("조준 완료");
                break;
            }
            else if (angleDifference > 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle + currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
            } 
            else if(angleDifference < 0)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(currentDir.x, currentAngle - currentRotationSpeed, currentDir.z));
                transform.rotation = newRotation;
            }

            currentRotationSpeed += rotationAddSpeed;

            //임시 코드
            moveSpeed = Mathf.Min(10f, moveSpeed+0.1f);

            Debug.Log($"현재 각도: {currentAngle} / 목표 각도: {targetAngle}");

            yield return new WaitForSeconds(0.1f);
        }
    }
}
