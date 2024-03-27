using UnityEngine;

/// <summary>
/// castPoint 지점의 텍스쳐 픽셀을 동적으로 감지하는 컴포넌트
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ColorCaster : MonoBehaviour {
    [SerializeField] private Transform castPoint;
    private SpriteRenderer spriteRenderer;
    private Texture2D texture;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        texture = spriteRenderer.sprite.texture;
        // 텍스처의 필터 모드를 Point로 설정하여 픽셀을 정확하게 읽을 수 있도록 합니다.
        texture.filterMode = FilterMode.Point;
    }

    public Color GetTopColor() {
        // 월드 좌표를 스프라이트의 텍스처 좌표로 변환합니다.
        var textureCoord = WorldToTextureCoords(castPoint.position);

        // 텍스처 좌표의 픽셀 색상을 가져옵니다.
        return texture.GetPixelBilinear(textureCoord.x / 100f, textureCoord.y / 100f);
    }

    private Vector2 WorldToTextureCoords(Vector3 worldPoint) {
        // 월드 좌표를 로컬 좌표로 변환합니다.
        var localPos = transform.InverseTransformPoint(worldPoint);
        
        // 로컬 좌표를 스프라이트의 텍스처 좌표로 변환합니다.
        var spriteRect = spriteRenderer.sprite.rect;
        var ppu = spriteRenderer.sprite.pixelsPerUnit;
        var spriteSize = spriteRect.size / ppu;
        var x = (localPos.x + spriteSize.x * 0.5f) * ppu / spriteSize.x;
        var y = (localPos.y + spriteSize.y * 0.5f) * ppu / spriteSize.y;
        return new Vector2(x, y);
    }
}