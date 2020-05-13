using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [TabGroup("Move")] [SerializeField] float walkSpeed;
    [TabGroup("Move")] [SerializeField] float runSpeed;
    [TabGroup("Move")] [SerializeField] float crouchSpeed;
    [TabGroup("Move")] [SerializeField] float jumpForce;
    [TabGroup("Move")] [SerializeField] float crouchPosY;
    [TabGroup("Move")] [ShowInInspector, DisableInEditorMode] float applySpeed;
    [TabGroup("Move")] float originPosY;
    [TabGroup("Move")] [ShowInInspector, DisableInEditorMode] float applyPosY;

    [BoxGroup("Condition")][ShowInInspector, DisableInEditorMode] bool isRun;
    [BoxGroup("Condition")] [ShowInInspector, DisableInEditorMode] bool isGround;
    [BoxGroup("Condition")] [ShowInInspector, DisableInEditorMode] bool isCrouch;

    [SerializeField] float lookSensitivity;
    [SerializeField] float cameraRotationLimit;
    [ShowInInspector, DisableInEditorMode] float currentCameraRotationX;


    [TabGroup("Components")] [SerializeField] Camera camera;
    [TabGroup("Components")] [SerializeField] Transform body;
    [TabGroup("Components")] [SerializeField] CapsuleCollider bodyCapsuleCollider;
    [TabGroup("Components")] [ShowInInspector, DisableInEditorMode] Rigidbody myRigid;
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        isRun = false;
        originPosY = camera.transform.localPosition.y;
        applyPosY = originPosY;
    }

    
    void Update()
    {
        Move();
        TryRun();
        IsGroundCheck();
        TryJump();
        TryCrouch();
        CameraRotation();
        PlayerRotation();
    }

    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch; //true면 false로, false면 true로

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyPosY = originPosY;
        }

        //Vector3 _cameraPos = new Vector3(camera.transform.localPosition.x, applyPosY, camera.transform.localPosition.z);
        //camera.transform.localPosition = _cameraPos;
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = camera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyPosY, 0.3f);
            if (count > 20)
            {
                _posY = applyPosY;
            }
            camera.transform.localPosition = new Vector3(0f, _posY, 0);
            
            yield return null;
        }
    }

    void IsGroundCheck()
    {
        isGround = Physics.Raycast(body.position, Vector3.down, bodyCapsuleCollider.bounds.extents.y + 0.05f);
    }

    void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    } 

    void Jump()
    {
        if (isCrouch) Crouch(); //앉은 상태에서 점프하면 앉은 상태 해제
        myRigid.velocity = transform.up * jumpForce;
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouch)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouch)
        {
            RunningCancel();
        }
    }

    void Running()
    {
        applySpeed = runSpeed;
        isRun = true;
    }

    void RunningCancel()
    {
        applySpeed = walkSpeed;
        isRun = false;
    }

    //이동
    void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    //상하 회전
    void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //좌우 회전
    void PlayerRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _playerRotationY = _yRotation * lookSensitivity;

        Vector3 _playerRotation = new Vector3(0f, _playerRotationY, 0f);

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_playerRotation));
    }
}
