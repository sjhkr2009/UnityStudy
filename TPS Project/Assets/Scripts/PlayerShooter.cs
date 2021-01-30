using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public enum AimState
    {
        Idle,   // 평시 모드. 플레이어 방향과 카메라 방향이 다를 수 있으며, 이동 시 플레이어는 카메라 방향을 기준으로 천천히 회전하며 이동한다.
        HipFire // 발사 모드. 플레이어 방향과 카메라 방향이 강제로 일치된다. FPS/TPS 게임에서 견착 상태로 표기된다.
    }

    public AimState aimState { get; private set; }

    public Gun gun;
    public LayerMask excludeTarget;
    
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Camera playerCamera;

    // 마지막 발사 시점에서 Idle 상태로 돌아갈 때까지의 시간
    private float releasingAimCooldown = 2.5f;
    private float lastFireInputTime;

    // 현재 방식에서는 총의 조준점이 카메라 중앙은 아니므로, 총알이 맞을 위치를 미리 저장해두기 위한 변수
    private Vector3 aimPoint;
    // 플레이어가 보는 방향과 카메라가 보는 방향 사이에 일정 수준 (여기서는 1도) 차이가 있다면 true를 반환한다.
    private bool linedUp => !(Mathf.Abs( playerCamera.transform.eulerAngles.y - transform.eulerAngles.y) > 1f);
    // 플레이어가 총을 발사할 수 있는지의 엽를 반환한다. 총구가 벽과 같은 다른 물체에 파묻혀 있다면 false를 반환한다.
    private bool hasEnoughDistance => !Physics.Linecast(transform.position + Vector3.up * gun.fireTransform.position.y, gun.fireTransform.position, ~excludeTarget);
    
    void Awake()
    {
        // 자신의 레이어는 조준 대상에서 제외시킨다.
        if (excludeTarget != (excludeTarget | (1 << gameObject.layer)))
            excludeTarget |= 1 << gameObject.layer;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        aimState = AimState.Idle;
        gun.gameObject.SetActive(true);
        gun.Setup(this);
    }

    private void OnDisable()
    {
        aimState = AimState.Idle;
        gun.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playerInput.Fire)
        {
            Shoot();
            lastFireInputTime = Time.time;
        }
        else if (playerInput.Reload)
        {
            Reload();
        }
    }

    private void Update()
    {
        UpdateAimTarget();
        UpdateUI();
        UpdateReleasingTime();

        playerAnimator.SetFloat("Angle", GetPlayerAngle());
    }

    public void Shoot()
    {
		switch (aimState)
		{
			case AimState.Idle:
                if(linedUp)
                    aimState = AimState.HipFire;
                break;

			case AimState.HipFire:
				if (hasEnoughDistance)
				{
                    if (gun.Fire(aimPoint))
                        playerAnimator.SetTrigger("Shoot");
                }
				else
				{
                    aimState = AimState.Idle;
				}
				break;

			default:
				break;
		}
    }

    public void Reload()
    {
        if(gun.Reload())
		{
            playerAnimator.SetTrigger("Reload");
		}
    }

    // 총알이 맞을 지점의 정보를 갱신한다. 총구에서 화면 중앙을 향해 발사하여 맞는 대상을 aimPoint로 지정한다.
    // 맞는 대상이 없으면 총구 앞 방향으로 최대 사정거리만큼 나아간 지점을 지정한다.
    private void UpdateAimTarget()
    {
        RaycastHit hit;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(Physics.Raycast(ray, out hit, gun.fireDistance, ~excludeTarget))
		{
            aimPoint = hit.point;

            if(Physics.Linecast(gun.fireTransform.position, hit.point, out hit, ~excludeTarget))
			{
                aimPoint = hit.point;
			}
		}
		else
		{
            aimPoint = playerCamera.transform.position + playerCamera.transform.forward * gun.fireDistance;
		}
    }

    private void UpdateUI()
    {
        if (gun == null || UIManager.Instance == null) 
            return;
        
        UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        
        UIManager.Instance.SetActiveCrosshair(hasEnoughDistance);
        UIManager.Instance.UpdateCrossHairPosition(aimPoint);
    }

    // IK가 갱신될 때마다 자동으로 호출된다.
    private void OnAnimatorIK(int layerIndex)
    {
        if (gun == null || gun.state == Gun.State.Reloading)
            return;

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandMount.rotation);
    }

    // 플레이어가 바라보는 상하 각도를 0(바닥을 볼 때) ~ 1(하늘을 볼 때) 사이의 값으로 반환한다.
    private float GetPlayerAngle()
	{
        float angle = playerCamera.transform.eulerAngles.x;
        if (angle > 270f)
            angle -= 360f;

        angle /= -180f; // 90도(바닥) ~ -90도(하늘)를 -0.5 ~ 0.5로 변환
        angle += 0.5f;  // 최종적으로 0 ~ 1 사이의 값으로 변환

        return angle;
    }

    private void UpdateReleasingTime()
	{
        if (aimState == AimState.Idle || playerInput.Fire)
            return;

        if (Time.time >= lastFireInputTime + releasingAimCooldown)
            aimState = AimState.Idle;
	}
}