using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    /// <summary>
    /// 지정한 원형 범위 내에서 NavMesh 위의 임의의 위치를 반환합니다.
    /// </summary>
    /// <param name="center">범위의 중심</param>
    /// <param name="distance">범위의 반지름</param>
    /// <returns></returns>
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask = NavMesh.AllAreas)
    {
        var randomPos = Random.insideUnitSphere * distance + center;
        
        NavMeshHit hit;

        // areaMask에 해당하는 NavMesh 위에서, 입력된 기준점에서 가장 가까운 지점을 반환한다.
        // 매개 변수: (기준점, out 반환값, 최대 탐색 거리, 탐색할 NavMesh)
        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);
        
        return hit.position;
    }

    /// <summary>
    /// 정규분포에 따른 랜덤값을 반환합니다. Box-Muller 변환 방식을 이용합니다.
    /// </summary>
    /// <param name="mean">평균값. 정규분포 그래프의 중심을 나타냅니다.</param>
    /// <param name="standard">표준편차. 높을수록 평균과 차이가 큰 값이 반환될 확률이 높아집니다.</param>
    /// <returns></returns>
    public static float GetRandomNormalDistribution(float mean, float standard)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        // 공식 참고: https://blog.naver.com/magatskami/90081492716
        float trans = Mathf.Sqrt(-2f * Mathf.Log(x1)) * Mathf.Sin(2f * Mathf.PI * x2);

        return mean + (standard * trans);
    }
}