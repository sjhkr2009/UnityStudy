using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public enum State
    {
        Normal,Invisible,Visible,Inactive
    }
    public State state;

    //LauncherMove
    Vector3 moveStart;
    Vector3 moveReturn;
    Vector3 targetPosition;
    public float speed;

    //CircleMove
    Vector3 centerPoint;
    Vector3 radiusLength;
    public bool isCircleMove;
    public float radius;

    public LaserManager laserManager;
    //public StageManager stageManager;


    bool isReturnGet;
    bool isReturn
    {
        set
        {
            if(value == true)
            {
                targetPosition = moveStart;
                isReturnGet = true;
            }
            else
            {
                targetPosition = moveReturn;
                isReturnGet = false;
            }
        }
        get
        {
            return isReturnGet;
        }
    }

    void Start()
    {
        if (!isCircleMove && transform.Find("StartPoint") != null && transform.Find("ReturnPoint") != null)
        {
            moveStart = transform.Find("StartPoint").transform.position;
            moveReturn = transform.Find("ReturnPoint").transform.position;
        }
        if (isCircleMove && transform.Find("CenterPoint") != null)
        {
            centerPoint = transform.Find("CenterPoint").transform.position;
            radiusLength = new Vector3(0, radius, 0);
        }
        isReturn = false;
    }

    void LauncherMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(transform.position == targetPosition)
        {
            if (isReturn)
            {
                isReturn = false;
            } else if (!isReturn)
            {
                isReturn = true;
            }
        }
    }

    void Update()
    {
        if (isCircleMove)
        {
            transform.RotateAround(centerPoint, new Vector3(0,0,1), speed * Time.deltaTime);
        }

        if (!isCircleMove && moveStart != moveReturn)
        {
            LauncherMove();
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("런처 클릭: " + transform.gameObject.name);
        laserManager.Hacking(transform.gameObject);
    }
}
