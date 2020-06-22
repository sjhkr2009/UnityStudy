using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [BoxGroup("Main Camera"), SerializeField] private Camera cam;

    [BoxGroup("Setting"), SerializeField] private float moveSpeed;
    [BoxGroup("Setting"), SerializeField] private float expansionSpeed;
    [BoxGroup("Setting"), SerializeField] private float minExpansion;
    [BoxGroup("Setting"), SerializeField] private float maxExpansion;

    [BoxGroup("Info"), SerializeField, ReadOnly] Vector2 previousPos;
    [BoxGroup("Info"), SerializeField, ReadOnly] Vector2 dragStartPoint;
    [BoxGroup("Info"), SerializeField, ReadOnly] float previousTouchDistance;
    [BoxGroup("Info"), SerializeField, ReadOnly] bool isDragging = false;

    private void Awake()
    {
        cam = Camera.main;
        previousPos = Vector2.zero;
        dragStartPoint = Vector2.zero;
        previousTouchDistance = 0;
    }

    private void Update()
    {
        if (isDragging) ScreenMove();
    }

    public void OnScreenDrag()
    {
        int touchCount = Input.touchCount;
        isDragging = true;
        Debug.Log(Input.mousePosition);
        if (touchCount == 1) ScreenMove();
        if (touchCount == 2) ScreenExpand();
    }

    public void ExitScreenDrag()
    {
        previousPos = Vector2.zero;
        dragStartPoint = Vector2.zero;
        previousTouchDistance = 0;
        isDragging = false;
    }

    void ScreenMove()
    {
        //Vector2 currentPos = Input.GetTouch(0).position;
        Vector2 currentPos = Input.mousePosition;
        if (previousPos != Vector2.zero)
        {
            Vector2 dir = (currentPos - previousPos);
            cam.transform.Translate(dir * moveSpeed * Time.deltaTime);
            previousPos = currentPos;
        }
        previousPos = currentPos;
    }

    void ScreenExpand()
    {
        float currentTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

        if (previousTouchDistance == 0) dragStartPoint = Vector2.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, 0.5f);
        else
        {
            float expansion = currentTouchDistance - previousTouchDistance;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + expansion * expansionSpeed, minExpansion, maxExpansion);

            Vector3 dir = new Vector3(dragStartPoint.x, dragStartPoint.y, cam.transform.position.z).normalized;
            cam.transform.position -= dir * Mathf.Abs(expansion);
        }
        previousTouchDistance = currentTouchDistance;
    }

    public void ExpandTest()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + scroll * expansionSpeed, minExpansion, maxExpansion);
    }
}
