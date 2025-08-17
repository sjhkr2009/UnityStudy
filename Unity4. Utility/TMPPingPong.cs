using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
[ExecuteAlways]
public class TMPPingPong : MonoBehaviour {
    const float Tolerance = 0.5f;
    
    public float moveSpeed = 80f;
    public float stopTimeOnEdge = 0.75f;
    public bool unscaledTime = true;

    TextMeshProUGUI textComponent;
    RectTransform rectTr;
    float currentOffset;

    bool isActive;

    Vector3[][] originVertices;
    float offsetMin;
    float offsetMax;
    string cachedText;
    float cachedRectWidth;

    int moveDir = 1;
    float pauseTime = 0f;
    float appliedOffset = 0f;

    // 스크롤 중엔 AutoSize를 끄고 최소 폰트로 "고정"했다가,
    // 스크롤이 필요 없어지면 원상복구하기 위한 플래그
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
        UnityEditor.EditorApplication.update += EditorUpdate;
#endif
    }

    void OnDisable() {
        ApplyOffset(offsetMin);
        RestoreAutoSize();
        
#if UNITY_EDITOR
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
            ApplyOffset(currentOffset);
            pauseTime -= deltaTime;
            return;
        }

        currentOffset += moveDir * moveSpeed * deltaTime;

        if (currentOffset >= offsetMax) {
            currentOffset = offsetMax;
            moveDir = -1;
            pauseTime = stopTimeOnEdge;
        }
        else if (currentOffset <= offsetMin) {
            currentOffset = offsetMin;
            moveDir = 1;
            pauseTime = stopTimeOnEdge;
        }

        ApplyOffset(currentOffset);
    }

    void RefreshData() {
        RestoreAutoSize();
        
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.overflowMode = TextOverflowModes.Masking;
        textComponent.alignment = TextAlignmentOptions.Left;

        textComponent.ForceMeshUpdate();

        offsetMin = 0f;
        appliedOffset = offsetMin;

        var rectWidth = rectTr.rect.width;
        cachedRectWidth = rectWidth;
        cachedText = textComponent.text;
        
        float textWidth;
        if (textComponent.enableAutoSizing) {
            var minSize = textComponent.fontSizeMin;
            
            var widthAtMinSize = CalcWidthAtMinSize(minSize);
            if (widthAtMinSize <= rectWidth + Tolerance) {
                // 크기만 줄여도 영역 내에 글자가 다 보일 때
                isActive = false;
                ApplyOffset(offsetMin);
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
        currentOffset = Mathf.Clamp(currentOffset, offsetMin, offsetMax);

        if (offsetMax <= Tolerance) {
            isActive = false;
            ApplyOffset(offsetMin);
            return;
        }

        CacheOriginVertices();
        isActive = true;
    }
    
    void ApplyOffset(float offset) {
        var info = textComponent.textInfo;
        int meshCount = info.meshInfo.Length;

        var deltaOffset = appliedOffset - offset;
        var shift = Vector3.right * deltaOffset;

        for (int i = 0; i < meshCount; i++) {
            var dst = info.meshInfo[i].vertices;
            //var src = originVertices[i];

            int len = dst.Length;
            for (int v = 0; v < len; v++)
                dst[v] += shift;// src[v] + shift;

            var mesh = info.meshInfo[i].mesh;
            mesh.vertices = dst;
            textComponent.UpdateGeometry(mesh, i);
        }
        
        appliedOffset = offset;
    }

    void CacheOriginVertices() {
        var info = textComponent.textInfo;
        int subCount = info.meshInfo.Length;
        if (originVertices == null || originVertices.Length != subCount)
            originVertices = new Vector3[subCount][];

        for (int i = 0; i < subCount; i++) {
            var src = info.meshInfo[i].vertices;
            if (originVertices[i] == null || originVertices[i].Length != src.Length)
                originVertices[i] = new Vector3[src.Length];
            
            Array.Copy(src, originVertices[i], src.Length);
        }
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
        if (!lockedAutoSize)
            return;

        lockedAutoSize = false;

        textComponent.enableAutoSizing = isAutoSize;
        textComponent.fontSize = cachedFontSize;
        textComponent.ForceMeshUpdate();
    }
}
