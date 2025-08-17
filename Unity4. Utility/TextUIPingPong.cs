using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
[ExecuteAlways]
public class TextUIPingPong : MonoBehaviour {
    const float Tolerance = 0.5f;

    public float moveSpeed = 60f;
    public float stopTimeOnEdge = 1.5f;
    public bool unscaledTime = true;
    public bool showOnEditor;
    
    public float CurrentOffset { get; private set; }

    TextMeshProUGUI textComponent;
    RectTransform rectTr;

    bool hasData;
    bool isActive;

    float offsetMin;
    float offsetMax;
    string cachedText;
    float cachedRectWidth;
    TextWrappingModes cachedWrapMode;
    TextOverflowModes cachedOverflowMode;
    TextAlignmentOptions cachedAlignment;

    int moveDir = 1;
    float pauseTime = 0f;

    bool lockedAutoSize;
    float cachedFontSize;
    bool isAutoSize;

    void Awake() {
        textComponent = GetComponent<TextMeshProUGUI>();
        rectTr = (RectTransform)transform;
    }

    void OnEnable() {
        RefreshData();
        ApplyOffset(offsetMin);

#if UNITY_EDITOR
        if (!Application.isPlaying) {
            UnityEditor.EditorApplication.update -= EditorUpdate;
            UnityEditor.EditorApplication.update += EditorUpdate;
        }
#endif
    }

    void OnDisable() {
        ApplyOffset(offsetMin);
        RestoreAutoSize();

#if UNITY_EDITOR
        if (!Application.isPlaying)
            UnityEditor.EditorApplication.update -= EditorUpdate;
#endif
    }

    void OnRectTransformDimensionsChange() {
        if (!rectTr)
            return;

        if (!Mathf.Approximately(cachedRectWidth, rectTr.rect.width))
            RefreshData();
    }

    bool IsDirty() {
        if (!textComponent)
            return false;

        if (textComponent.havePropertiesChanged || cachedText != textComponent.text)
            return true;

        if (lockedAutoSize && textComponent.enableAutoSizing)
            return true;

        return false;
    }

    void LateUpdate() {
        if (!Application.isPlaying)
            return;

        var deltaTime = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        OnUpdate(deltaTime);
    }

    void EditorUpdate() {
#if UNITY_EDITOR
        if (Application.isPlaying)
            return;

        if (!showOnEditor)
        {
            SetDisable();
            return;
        }

        if (!hasData)
            RefreshData();
        
        OnUpdate(Time.unscaledDeltaTime);
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
        UnityEditor.SceneView.RepaintAll();
#endif
    }

    void OnUpdate(float deltaTime) {
        if (IsDirty())
            RefreshData();

        if (!isActive)
            return;

        if (pauseTime > 0f) {
            ApplyOffset(CurrentOffset);
            pauseTime -= deltaTime;
            return;
        }

        var targetOffset = CurrentOffset + (moveDir * moveSpeed * deltaTime);

        if (targetOffset >= offsetMax) {
            targetOffset = offsetMax;
            moveDir = -1;
            pauseTime = stopTimeOnEdge;
        }
        else if (targetOffset <= offsetMin) {
            targetOffset = offsetMin;
            moveDir = 1;
            pauseTime = stopTimeOnEdge;
        }

        ApplyOffset(targetOffset);
    }

    void RefreshData() {
        ResetLocalData();

        if (!textComponent)
            return;

        if (!Application.isPlaying && !showOnEditor)
            return;

        cachedWrapMode = textComponent.textWrappingMode;
        cachedOverflowMode = textComponent.overflowMode;
        cachedAlignment = textComponent.alignment;

        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.overflowMode = TextOverflowModes.Masking;
        textComponent.alignment = TextAlignmentOptions.Left;

        textComponent.ForceMeshUpdate();

        offsetMin = 0f;
        CurrentOffset = offsetMin;

        hasData = true;

        var rectWidth = rectTr.rect.width;
        cachedRectWidth = rectWidth;
        cachedText = textComponent.text;

        float textWidth;
        if (textComponent.enableAutoSizing) {
            var minSize = textComponent.fontSizeMin;

            var widthAtMinSize = CalcWidthAtMinSize(minSize);
            if (widthAtMinSize <= rectWidth + Tolerance) {
                // 크기만 줄여도 영역 내에 글자가 다 보일 때
                SetDisable();
                return;
            }
            else {
                LockAutoSize();

                textWidth = widthAtMinSize;
                textComponent.ForceMeshUpdate();
            }
        }
        else {
            textWidth = textComponent.GetPreferredValues(textComponent.text, Mathf.Infinity, Mathf.Infinity).x;
        }

        offsetMax = Mathf.Max(offsetMin, textWidth - rectWidth);
        CurrentOffset = Mathf.Clamp(CurrentOffset, offsetMin, offsetMax);

        if (offsetMax <= Tolerance) {
            SetDisable();
            return;
        }

        isActive = true;
    }

    void SetDisable() {
        isActive = false;
        
        ApplyOffset(offsetMin);
        ResetLocalData();
    }

    void ApplyOffset(float offset) {
        if (!hasData)
            return;

        if (Mathf.Approximately(offset, CurrentOffset))
            return;

        var info = textComponent.textInfo;
        int meshCount = info.meshInfo.Length;

        var deltaOffset = CurrentOffset - offset;
        var shift = Vector3.right * deltaOffset;

        for (int i = 0; i < meshCount; i++) {
            var dst = info.meshInfo[i].vertices;
            if (dst == null)
                continue;

            int len = dst.Length;
            for (int v = 0; v < len; v++)
                dst[v] += shift;
        }
        
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

        CurrentOffset = offset;
    }

    float CalcWidthAtMinSize(float fontSize) {
        // 화면에 영향 안 주고 너비만 계산해야 하므로, 이전 크기로 복귀시키기 전에 ForceMeshUpdate를 호출하면 안 됨.
        var prevAuto = textComponent.enableAutoSizing;
        var prevSize = textComponent.fontSize;

        textComponent.enableAutoSizing = false;
        textComponent.fontSize = fontSize;

        var width = textComponent.GetPreferredValues(textComponent.text, Mathf.Infinity, Mathf.Infinity).x;

        textComponent.fontSize = prevSize;
        textComponent.enableAutoSizing = prevAuto;
        return width;
    }

    void LockAutoSize() {
        if (lockedAutoSize)
            return;

        lockedAutoSize = true;

        var minSize = textComponent.fontSizeMin;

        isAutoSize = textComponent.enableAutoSizing;
        cachedFontSize = textComponent.fontSize;

        textComponent.enableAutoSizing = false;
        textComponent.fontSize = minSize;
    }

    void RestoreAutoSize() {
        if (!hasData)
            return;

        if (!lockedAutoSize)
            return;

        lockedAutoSize = false;

        textComponent.enableAutoSizing = isAutoSize;
        textComponent.fontSize = cachedFontSize;
        textComponent.ForceMeshUpdate();
    }

    void ResetLocalData() {
        if (!hasData)
            return;

        textComponent.textWrappingMode = cachedWrapMode;
        textComponent.overflowMode = cachedOverflowMode;
        textComponent.alignment = cachedAlignment;

        RestoreAutoSize();
        pauseTime = 0f;
        moveDir = 1;

        hasData = false;
    }
}
