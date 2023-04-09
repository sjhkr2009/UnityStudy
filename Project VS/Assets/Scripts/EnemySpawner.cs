using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float spawnDelay = 0.2f;

    private float timer = 0f;

    private void Start() {
        if (spawnPoint == null || spawnPoint.Length == 0) {
            Debug.LogError($"[EnemySpawner.Start] SpawnPoint is Empty!!");
            spawnPoint = new[] { transform };
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > spawnDelay) {
            timer -= spawnDelay;
            Spawn();
        }
    }

    void Spawn() {
        var point = spawnPoint.PickRandom();
        var enemy = PoolManager.Get($"Enemy{Random.Range(1, 3):00}");
        enemy.transform.position = point.position;

    }
}
