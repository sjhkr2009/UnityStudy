using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour {
    
    public Rigidbody2D player;
    
    public float speed = 3f;
    Vector2 mousePos;
    Vector2 size;
    bool isClick;
    
    void Start()
    {
        size = transform.localScale;
        isClick = false;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickDistance = new Vector2 (Mathf.Abs(mousePos.x - transform.position.x), Mathf.Abs(mousePos.y - transform.position.y));
            if (clickDistance.x < (size.x * 2.5f) && clickDistance.y < (size.y * 2.5f))
            {
                isClick = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            transform.localPosition = Vector2.zero;
            player.velocity = Vector2.zero;
        }

        if (isClick)
        {
            Move();
            UIDrag();
        }
    }

    public void Move()
    {
        Vector2 inputMove = transform.localPosition * speed;
        player.velocity = inputMove;
    }

    void UIDrag()
    {
        transform.position = mousePos;
        
        if (transform.parent.lossyScale.x / 2 < Vector2.Distance(transform.parent.position, transform.position))
        {
            Vector2 currentPos = transform.localPosition;
            Vector2 currentDistance = currentPos - Vector2.zero;
            currentDistance.Normalize();

            transform.localPosition = currentDistance / 2;
        }
        
    }
}