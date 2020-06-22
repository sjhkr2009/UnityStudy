using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    Vector2 mousePos;
    Vector2 clickPoint;
    float maxDistance;
    public bool isMoving;
    
    void Start()
    {
        isMoving = false;
        maxDistance = transform.parent.lossyScale.x / 2;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            clickPoint = mousePos;
            RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);

            if (hit.collider == null)
            {
                return;
            }

            if(hit.collider.tag == "MoveUI")
            {
                isMoving = true;
                Debug.Log("Move On");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            transform.localPosition = Vector2.zero;
            Debug.Log("Move Off");
        }

        if (isMoving)
        {
            UIMove();
        }
    }

    void UIMove()
    {
        transform.position = mousePos;

        if (maxDistance < Vector2.Distance(transform.parent.position, transform.position))
        {
            Vector2 currentPos = transform.localPosition;
            Vector2 currentDistance = currentPos - Vector2.zero;
            currentDistance.Normalize();

            transform.localPosition = currentDistance / 2;
        }
    }

    public Vector2 PlayerMove()
    {
        return transform.localPosition;
    }
}
