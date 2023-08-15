using UnityEngine;

public static class ProjectileSpawner {
    public static void SpawnAround(string prefabName, ProjectileParam param, int count, float maxAngle) {
        var anglePerCount = count <= 1 ? 0f : maxAngle / (count - 1);
        var startAngle = -(maxAngle * 0.5f);
        var originDirection = param.direction;
        
        for (int i = 0; i < count; i++) {
            var projectile = PoolManager.Get<Projectile>(prefabName);
            projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, param.direction); // y축을 기준으로 dir을 바라봄
            var angleDiff = startAngle + (anglePerCount * i);
            projectile.transform.Rotate(Vector3.forward * angleDiff);

            param.direction = GetRotatedVector(originDirection, angleDiff);
            
            projectile.Initialize(param);
        }
    }
    
    /** 주어진 벡터값을 일정량 회전시킨 방향 벡터를 반환합니다. */
    private static Vector2 GetRotatedVector(Vector2 origin, float angleDegrees) {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angleRadians);
        float cos = Mathf.Cos(angleRadians);
        
        float rotatedX = cos * origin.x - sin * origin.y;
        float rotatedY = sin * origin.x + cos * origin.y;

        return new Vector2(rotatedX, rotatedY).normalized;
    }
    
    public static Projectile Spawn(string prefabName, ProjectileParam param) {
        var projectile = PoolManager.Get<Projectile>(prefabName);
        projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, param.direction); // y축을 기준으로 dir을 바라봄
        projectile.Initialize(param);

        return projectile;
    }
}
