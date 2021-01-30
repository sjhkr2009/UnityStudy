using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();

    public float damageMax = 40f;
    public float damageMin = 20f;
    public Enemy enemyPrefab;

    public float healthMax = 200f;
    public float healthMin = 100f;

    public Transform[] spawnPoints;

    public float speedMax = 12f;
    public float speedMin = 3f;

    public Color strongEnemyColor = Color.red;
    private int wave;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover) 
            return;
        
        if (enemies.Count <= 0) 
            SpawnWave();
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateWaveText(wave, enemies.Count);
    }
    
    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 5f);

		for (int i = 0; i < spawnCount; i++)
		{
            float intensity = Random.value;
            CreateEnemy(intensity);
		}
    }
    
    /// <summary>
    /// 적 오브젝트를 생성합니다.
    /// </summary>
    /// <param name="intensity">0~1 사이의 값으로, 높을수록 적의 능력치가 높아집니다.</param>
    private void CreateEnemy(float intensity)
    {
        intensity = Mathf.Clamp(intensity, 0f, 1f);

        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnTrans = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnTrans.position, spawnTrans.rotation);
        enemy.Setup(health, damage, speed, speed * 0.3f, skinColor);
        enemies.Add(enemy);

        enemy.OnDeath += () => { enemies.Remove(enemy); };
        enemy.OnDeath += () => { Destroy(enemy.gameObject, 10f); };
        enemy.OnDeath += () => { GameManager.Instance.AddScore(100); };
    }
}