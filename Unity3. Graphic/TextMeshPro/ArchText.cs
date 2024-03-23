using TMPro;
using UnityEngine;

[ExecuteAlways]
public class ArchText : MonoBehaviour {
    [SerializeField, AutoAssignComponent] private TMP_Text textComponent;
    
    [SerializeField] private bool refreshOnUpdate;
    [SerializeField] private float maxAngle = 30f;

    private void Start() {
        textComponent ??= GetComponent<TMP_Text>();
        ArrangeText();
    }
    
    private void Update() {
#if UNITY_EDITOR
        if (!Application.isPlaying) {
            ArrangeText();
        }
#endif
        if (refreshOnUpdate) {
            ArrangeText();
        }
    }

    public void ArrangeText() {
        if (!textComponent) return;
        
        textComponent.ForceMeshUpdate();
    
        var textInfo = textComponent.textInfo;

        float centerX = (textInfo.characterCount - 1) * 0.5f;
        if (centerX == 0) return;
        
        float angleDiff = maxAngle / centerX;
        var first = textInfo.characterInfo[0];
        var last = textInfo.characterInfo[textInfo.characterCount - 1];
        var firstVertices = textInfo.meshInfo[first.materialReferenceIndex].vertices;
        var lastVertices = textInfo.meshInfo[last.materialReferenceIndex].vertices;
        var centerPos = (firstVertices[first.vertexIndex] + lastVertices[last.vertexIndex + 2]) * 0.5f;
    
        for (int i = 0; i < textInfo.characterCount; i++) {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
        
            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            var vertices = meshInfo.vertices;
            
            var charCenter = (vertices[charInfo.vertexIndex] + vertices[charInfo.vertexIndex + 2]) * 0.5f;
            
            var distFromCenter = centerX - i;
            
            var targetAngle = distFromCenter * angleDiff;
            var rotatedCenter = GetRotatedPosition(centerPos, charCenter, targetAngle);

            for (int k = 0; k < 4; k++) {
                var vertexIndex = charInfo.vertexIndex + k;
                var vertex = vertices[vertexIndex];

                // 각도 조정
                var rotatedPoint = RotatePoint(vertex, rotatedCenter, targetAngle);
                vertices[vertexIndex] = rotatedPoint;
            }
        }
    
        textComponent.UpdateVertexData();
    }
    
    private Vector2 RotatePoint(Vector2 point, Vector2 center, float angle) {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        Vector2 distance = point - center;
        float rotatedX = distance.x * cos - distance.y * sin;
        float rotatedY = distance.x * sin + distance.y * cos;

        return center + new Vector2(rotatedX, rotatedY);
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