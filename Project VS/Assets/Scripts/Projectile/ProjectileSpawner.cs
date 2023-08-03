using UnityEngine;

public static class ProjectileSpawner {
    public static Projectile SpawnStraight(string prefabName, float damage, Vector2 spawnPoint, Vector2 direction, float speed = 10f) {
        var param = ProjectileParam.CreateStraightDefault(spawnPoint);
        param.damage = damage;
        param.speed = speed;
        param.direction = direction;
        return Spawn(prefabName, param);
    }
    
    public static Projectile Spawn(string prefabName, ProjectileParam param) {
        var projectile = PoolManager.Get<Projectile>(prefabName);
        projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, param.direction); // y축을 기준으로 dir을 바라봄
        projectile.Initialize(param);

        return projectile;
    }
}
