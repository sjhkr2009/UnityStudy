using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    private float screenView;

    public float cameraBasicSize;
    public float sizeUpPerSecond;
    public float cameraMaxSize;
    public float CameraZPos => transform.position.z;
    public float CameraYPos => transform.position.y;
    public float CameraXSize => mainCamera.orthographicSize * screenView;
    public float CameraYSize => mainCamera.orthographicSize;

    void Awake()
    {
        mainCamera = Camera.main;
        mainCamera.orthographicSize = cameraBasicSize;
    }

    private void Start()
    {
        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;
    }

    public void SizeUpPerSecond()
    {
        mainCamera.orthographicSize = Mathf.Min(mainCamera.orthographicSize + sizeUpPerSecond, cameraMaxSize);
    }
}
