using UnityEngine;
using UnityEngine.AI;

// 플레이어의 컴포넌트들을 총괄하는 클래스
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public int lifeRemains = 3;
    private AudioSource playerAudioPlayer;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    void GetPlayerComnponents()
	{
        animator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    private void Start()
    {
        GetPlayerComnponents();

        playerHealth.OnDeath -= HandleDeath;
        playerHealth.OnDeath += HandleDeath;

        UIManager.Instance.UpdateLifeText(lifeRemains);

        Cursor.visible = false;
    }
    
    private void HandleDeath()
    {
        playerMovement.enabled = false;
        playerShooter.enabled = false;

        if(lifeRemains > 0)
		{
            lifeRemains--;
            UIManager.Instance.UpdateLifeText(lifeRemains);
            Invoke(nameof(Respawn), 3f);
		}
		else
		{
            GameManager.Instance.EndGame();
		}
        
        Cursor.visible = true;
    }

    public void Respawn()
    {
        // 각 스크립트의 OnEnable / OnDisable을 통해 초기화를 관리하고, 여기서는 껐다 켜는 동작만 해 준다.
        gameObject.SetActive(false);
        transform.position = Utility.GetRandomPointOnNavMesh(transform.position, 30f, NavMesh.AllAreas);

        playerMovement.enabled = true;
        playerShooter.enabled = true;
        gameObject.SetActive(true);

        playerShooter.gun.ammoRemain = 120;

        Cursor.visible = false;
    }
}