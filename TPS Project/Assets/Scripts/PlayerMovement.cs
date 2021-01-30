using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;
    private Animator animator;
    
    private Camera followCam;
    
    public float speed = 6f;
    public float jumpVelocity = 20f;
    [Range(0.01f, 1f)] public float airControlPercent;

    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.05f;
    
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;
    
    private float currentVelocityY;
    
    public float CurrentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
        animator = GetComponent<Animator>();

        followCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (CurrentSpeed > 0.2f || playerInput.Fire || playerShooter.aimState == PlayerShooter.AimState.HipFire)
            Rotate();

        Move(playerInput.MoveInput);
        
        if (playerInput.Jump)
            Jump();
    }

    private void Update()
    {
        UpdateAnimation(playerInput.MoveInput);
    }

    // 상하좌우 방향 조작을 입력받아서, Damping과 Gravity를 적용하여 해당 방향으로 캐릭터를 이동시킨다.
    public void Move(Vector2 moveInput)
    {
        float targetSpeed = speed * moveInput.magnitude;
        Vector3 moveDir = ((transform.forward * moveInput.y) + (transform.right * moveInput.x)).normalized;

        // 조작에 따라 움직이는 정도를 결정한다. airControlPercent가 낮을수록 공중에서의 지연시간을 늘려 조작의 영향을 덜 받게 한다. (1이면 지상-공중 동일)
        float smoothTime = characterController.isGrounded ?
            (speedSmoothTime) : (speedSmoothTime / airControlPercent);

        // Vector/Mathf::SmoothDamp : (현재 값, 목표 값, ref 현재 변화량, 지연시간, (옵션)최대 속도, (옵션)이 함수를 이전에 호출한 시간)을 입력한다. 현재 값에서 목표 값으로 시간에 따라 변화하는 값을 반환한다.
        // 최대 속도와 호출 시간은 기본값이 각각 무제한, Time.deltatime으로 지정되어 있으며, 호출 시간에 따라 현재 속도를 계산하여 현재 변화량을 수정한다.
        targetSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        // Rigidbody가 없으므로 코드를 통해 중력의 영향을 받게 한다.
        // Physics.gravity : Rigidbody에 적용되는 중력값
        currentVelocityY += Physics.gravity.y * Time.deltaTime;

        // 최종 속도값을 구한다. (방향 * 속도 + 중력의 영향)
        Vector3 velocity = moveDir * targetSpeed + Vector3.up * currentVelocityY;

        characterController.Move(velocity * Time.deltaTime);

        // 땅에 닿았을 경우 중력에 의한 Y속도에 의해 땅을 뚫고 내려가는 현상 방지
        if (characterController.isGrounded)
            currentVelocityY = 0f;
    }

    // 카메라 방향에 따라 캐릭터의 Y축을 회전시킨다. (카메라는 마우스 조작을 통해 움직인다)
    public void Rotate()
    {
        float targetRot = followCam.transform.eulerAngles.y;

        // SmoothDampAngle : SmoothDamp와 동일한 기능이지만 각도의 특수성을 고려한다. 예를 들어 30도 -> 300도로 회전을 명령하면 270도를 도는 대신, 반대 방향으로 90도 회전한다.
        targetRot = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVelocity, turnSmoothTime);

        transform.eulerAngles = Vector3.up * targetRot;
    }

    public void Jump()
    {
        if (!characterController.isGrounded)
            return;

        currentVelocityY = jumpVelocity;
    }

    
    private void UpdateAnimation(Vector2 moveInput)
    {
        // 설정된 속도와 실제 속도의 비율을 구해서, 실제 속도에 맞게 입력값을 제어한다.
        float animSpeedPercent = CurrentSpeed / speed;

        // 파라미터 값이 부드럽게 변화하도록 dampTime을 넣는다.
        // dampTime은 값이 변화하는 시간, deltaTime은 SetFloat을 마지막으로 호출한 시간을 의미한다. (매 프레임 호출되므로 Time.deltaTime 입력)
        animator.SetFloat("Vertical Move", moveInput.y * animSpeedPercent, 0.05f, Time.deltaTime);
        animator.SetFloat("Horizontal Move", moveInput.x * animSpeedPercent, 0.05f, Time.deltaTime);
    }
}