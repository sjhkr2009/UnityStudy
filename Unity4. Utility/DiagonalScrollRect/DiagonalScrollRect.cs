using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 요소들이 세로로 스크롤될 때, 사선으로 움직이게 해 주는 ScrollRect 확장 컴포넌트
/// </summary>
[ExecuteAlways]
public class DiagonalScrollRect : ScrollRect {
    [SerializeField, Range(-1f, 1f)] private float angle = 0f;

    private bool isInit;

    protected override void SetContentAnchoredPosition(Vector2 position) {
        Vector2 newPos = position;
        newPos.x = -newPos.y * angle;

        base.SetContentAnchoredPosition(newPos);
    }

    private void InitializeIfNotInitialize() {
        if (isInit) return;

        if (!content) return;
        
        // content의 자식들이 중심부에 정렬되게 한다.
        content.anchorMin = new Vector2(0.5f, content.anchorMin.y);
        content.anchorMax = new Vector2(0.5f, content.anchorMax.y);
        content.anchoredPosition = new Vector2(0f, content.anchoredPosition.y);
        vertical = true;
        horizontal = false;

        isInit = true;
    }

    protected override void LateUpdate() {
        InitializeIfNotInitialize();
        
        if (!isInit) return;
        
        base.LateUpdate();
        
        float scrollHeight = viewport.rect.height;
        float scrollCenterY = viewport.rect.y + scrollHeight / 2f;

        for (int i = 0; i < content.childCount; i++) {
            var child = content.GetChild(i) as RectTransform;
            if (child != null) {
                var originPos = child.anchoredPosition;

                // 자식의 y좌표
                float childCenterY = content.anchoredPosition.y + originPos.y + child.rect.height / 2;
                float yOffsetFromCenter = childCenterY - scrollCenterY;

                // y좌표에 비례하여 x좌표 조정
                originPos.x = yOffsetFromCenter * angle;
                child.anchoredPosition = originPos;
            }
        }
    }
}
