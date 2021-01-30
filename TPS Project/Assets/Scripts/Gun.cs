using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; }

    // 총을 들고 있는 오브젝트
    private PlayerShooter _gunHolder;

    // 총알 궤적 이펙트 그리기용
    private LineRenderer _bulletLineRenderer;
    
    // Audio: 총알 발사 및 재장전 소리 재생
    private AudioSource _gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    
    // Particle: 출력할 파티클 이펙트
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    
    // Transform: 총알 발사 위치와 플레이어 왼손 위치
    public Transform fireTransform;
    public Transform leftHandMount;

    // 총기: 총의 데미지와 사정거리
    public float damage = 25;
    public float fireDistance = 100f;

    // 탄창: 탄약 수, 현재 장전된 탄약 수, 탄창 용량
    public int ammoRemain = 100;
    public int magAmmo;
    public int magCapacity = 30;

    // 딜레이: 발사 간격 및 재장전 시간
    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;
    
    // 탄 퍼짐: 탄 퍼짐 최대치, 안정성(반동), 탄 퍼짐 감소 속도, 현재 탄 퍼짐 정도, 현재 탄 퍼짐 변화량 기록 변수
    [Range(0f, 10f)] public float maxSpread = 3f;
    [Range(1f, 10f)] public float stability = 1f;
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;
    private float currentSpread;
    private float currentSpreadVelocity;

    // 최근 총알 발사 시점 기록용
    private float lastFireTime;

    // 총을 든 플레이어는 총에 맞지 않게 처리하기 위한 LayerMask
    private LayerMask excludeTarget;

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();
        _gunAudioPlayer = GetComponent<AudioSource>();

        // 궤적은 총구와 적중한 지점을 연결하게 되므로, 2개의 점을 잇는 라인렌더러가 필요하다.
        _bulletLineRenderer.positionCount = 2;
        _bulletLineRenderer.enabled = false;
    }

    // 총의 주인을 설정한다.
    public void Setup(PlayerShooter gunHolder)
    {
        _gunHolder = gunHolder;
        excludeTarget = gunHolder.excludeTarget;
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
        currentSpread = 0f;
        lastFireTime = 0f;
        state = State.Ready;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // 외부에서 발사 명령을 받는 부분
    public bool Fire(Vector3 aimTarget)
    {
        if (state == State.Ready && magAmmo <= 0)
            state = State.Empty;

        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
		{
            Vector3 fireDirection = (aimTarget - fireTransform.position);

            // 정규분포를 이용한 탄 퍼짐 구현
            //------------------------------------------------------------------------------------
            float spreadX = Utility.GetRandomNormalDistribution(0f, currentSpread);
            float spreadY = Utility.GetRandomNormalDistribution(0f, currentSpread);

            // Quaternion.AngleAxis(angle, axis) :     axis를 축으로 angle 만큼 회전한 회전값을 반환한다.
            // Quaternion * Vector3(연산자 오버로딩) : 해당 벡터를 쿼터니언의 회전값만큼 회전시킨 후, 벡터를 반환한다.
            fireDirection = Quaternion.AngleAxis(spreadY, Vector3.up) * fireDirection;
            fireDirection = Quaternion.AngleAxis(spreadX, Vector3.right) * fireDirection;
            //------------------------------------------------------------------------------------

            // 안정성에 반비례하게 탄 퍼짐의 정도를 증가시킨다. (안정성이 낮을수록 반동이 커짐)
            currentSpread = Mathf.Min((currentSpread + 1f / stability), maxSpread);

            lastFireTime = Time.time;
            Shot(fireTransform.position, fireDirection);

            return true;
        }
        else return false;
    }
    
    // 실제 발사 동작 구현부
    private void Shot(Vector3 startPoint, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 hitPosition;

        if (Physics.Raycast(startPoint, direction, out hit, fireDistance, ~excludeTarget))
		{
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
			{
                DamageMessage damageMessage;

                damageMessage.attacker = _gunHolder.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;

                target.ApplyDamage(damageMessage);
			}
            else
			{
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);
			}
            hitPosition = hit.point;
		}
        else
		{
            hitPosition = startPoint + (direction * fireDistance);
		}

        StartCoroutine(ShotEffect(hitPosition));

        if (--magAmmo <= 0)
            state = State.Empty;
    }

    // 발사 이펙트 출력
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        _gunAudioPlayer.PlayOneShot(shotClip);

        _bulletLineRenderer.enabled = true;
        _bulletLineRenderer.SetPosition(0, fireTransform.position);
        _bulletLineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);

        _bulletLineRenderer.enabled = false;
    }
    
    // 외부에서 재장전 명령을 받는 부분
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
            return false;

        StartCoroutine(nameof(ReloadRoutine));
        return true;
    }
    
    // 실제 재장전 동작 구현부
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;

        _gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = Mathf.Clamp(magCapacity - magAmmo, 0, ammoRemain);
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }

    private void Update()
    {
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);
    }
}