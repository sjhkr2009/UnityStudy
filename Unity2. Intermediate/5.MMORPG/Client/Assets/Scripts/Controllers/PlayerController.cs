using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public class PlayerController : BaseController {
    protected override void Init() {
        base.Init();
        transform.position = GridMap.CellToWorld(cellPos);
    }

    private void Update() {
        UpdateDirInput();
        UpdateController();
    }

    private void LateUpdate() {
        // TODO: 테스트용 코드. 카메라 관리 스크립트 따로 만들 것.
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void UpdateDirInput()
    {
        if (Input.GetKey(KeyCode.W)) {
            SetDirection(MoveDir.Up);
        } else if (Input.GetKey(KeyCode.S)) {
            SetDirection(MoveDir.Down);
        } else if (Input.GetKey(KeyCode.D)) {
            SetDirection(MoveDir.Right);
        } else if (Input.GetKey(KeyCode.A)) {
            SetDirection(MoveDir.Left);
        } else {
            SetDirection(MoveDir.None);
        }
    }
}
