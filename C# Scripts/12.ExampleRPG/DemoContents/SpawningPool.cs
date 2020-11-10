using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class SpawningPool : MonoBehaviour
{
    [SerializeField, ReadOnly] int _monsterCount = 0;
	[SerializeField, ReadOnly] int _reservedCount = 0;
	[SerializeField] int _maxMonsterCount = 0;

    [SerializeField] Vector3 _spawnPos;
    [SerializeField] float _spawnRadius = 15f;
    [SerializeField] float _spawnTime = 5f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetMaxCount(int count) { _maxMonsterCount = count; }

	private void Start()
	{
        GameManager.Game.OnSpawnEvent -= AddMonsterCount;
        GameManager.Game.OnSpawnEvent += AddMonsterCount;
	}

	private void Update()
	{
		while(_monsterCount + _reservedCount < _maxMonsterCount)
		{
			StartCoroutine(nameof(ReserveSpawn));
		}
	}

	IEnumerator ReserveSpawn()
	{
		_reservedCount++;
		yield return new WaitForSeconds(Random.Range(0, _spawnTime));

		GameObject obj = GameManager.Game.Spawn(Define.ObjectType.Monster, "Knight");
		NavMeshAgent nav = obj.GetOrAddComponent<NavMeshAgent>();

		Vector3 randPos = _spawnPos;

		while (true)
		{
			yield return null;
			randPos += (Random.insideUnitSphere * Random.Range(0, _spawnRadius));
			randPos.y = 0;

			NavMeshPath path = new NavMeshPath();
			if (nav.CalculatePath(randPos, path))
				break;
		}

		obj.transform.position = randPos;
		_reservedCount--;
	}
}
