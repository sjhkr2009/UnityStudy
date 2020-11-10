using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    enum CursorType
    {
        None,
        Idle,
        Attack
    }
    CursorType _cursorType = CursorType.None;
    Texture2D cursorArrow;
    Texture2D cursorAttack;
    Texture2D cursorHand;

    void Start()
    {
        cursorArrow = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Arrow");
        cursorAttack = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        cursorHand = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f, _mask))
        {

            switch (hit.collider.gameObject.layer)
            {
                case (int)Define.Layer.Ground:
                    if (_cursorType != CursorType.Idle)
                    {
                        Cursor.SetCursor(cursorHand, new Vector2(cursorArrow.width / 4, 0), CursorMode.Auto);
                        _cursorType = CursorType.Idle;
                    }
                    break;
                case (int)Define.Layer.Monster:
                    if (_cursorType != CursorType.Attack)
                    {
                        Cursor.SetCursor(cursorAttack, new Vector2(cursorArrow.width / 5, 0), CursorMode.Auto);
                        _cursorType = CursorType.Attack;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
