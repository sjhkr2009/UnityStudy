using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

// Temp: 아치형 텍스트 연구중...
public class ArchText : MonoBehaviour {

    public TMP_Text textComponent;

    [Button]
    public void ArrangeText() {
        textComponent.ForceMeshUpdate();
    
        var textInfo = textComponent.textInfo;
        float radius = 5f;
        float maxAngle = 30f;
        
        float centerX = (textInfo.characterCount - 1) * 0.5f;
        if (centerX == 0) return;
        
        float angleDiff = maxAngle / centerX;
        var centerInfo = textInfo.characterInfo[(textInfo.characterCount / 2)];
        var centerVertices = textInfo.meshInfo[centerInfo.materialReferenceIndex].vertices;
        var centerPos = (centerVertices[centerInfo.vertexIndex] + centerVertices[centerInfo.vertexIndex + 2]) * 0.5f;
    
        for (int i = 0; i < textInfo.characterCount; i++) {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
        
            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            var vertices = meshInfo.vertices;
            
            var charCenter = (vertices[charInfo.vertexIndex] + vertices[charInfo.vertexIndex + 2]) * 0.5f;
            
            var distFromCenter = Mathf.Abs(i - centerX);
            var distFromEndPoint = textInfo.characterCount - distFromCenter;

            for (int k = 0; k < 4; k++) {
                var vertexIndex = charInfo.vertexIndex + k;
                var vertex = vertices[vertexIndex];

                // 끝 부분과의 거리에 따라 y 좌표 조정
                //var height = distFromEndPoint * radius;
                //var posDiff = Vector3.up * height;

                // 각도 조정
                var targetAngle = distFromCenter * angleDiff * (i < centerX ? 1 : -1);
                var rotatedCenter = GetRotatedPosition(centerPos, charCenter, targetAngle);
                Debugger.Log($"[Rotate] Pos {charCenter} -> {rotatedCenter}");
                var rotatedPoint = RotatePoint(vertex, rotatedCenter, targetAngle);

                vertices[vertexIndex] = rotatedPoint;// + posDiff;
            }
        }
    
        textComponent.UpdateVertexData();
    }

    private Vector3 RotatePoint(Vector3 point, Vector3 center, float angle) {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        float x = point.x - center.x;
        float y = point.y - center.y;
        float z = point.z - center.z;

        // 중심을 기준으로 회전 변환
        float rotatedX = x * cos - y * sin;
        float rotatedY = x * sin + y * cos;

        return new Vector3(rotatedX + center.x, rotatedY + center.y, z + center.z);
    }

    private Vector2 GetRotatedPosition(Vector2 center, Vector2 edge, float angle) {
        Vector2 midpoint = (center + edge) / 2f;
        Vector2 direction = (edge - center).normalized;
        
        float radians = angle * Mathf.Deg2Rad;

        // 회전 변환 행렬 계산
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // 회전된 좌표 계산
        Vector2 rotatedPoint = new Vector2(
            midpoint.x + (cos * direction.x - sin * direction.y),
            midpoint.y + (sin * direction.x + cos * direction.y)
        );
        return rotatedPoint;
    }
}
