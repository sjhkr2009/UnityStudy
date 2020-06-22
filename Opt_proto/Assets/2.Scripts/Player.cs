using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseUnit
{
    public void PlayerMove(Vector2 velocity)
    {
        transform.Translate(velocity * moveSpeed * Time.deltaTime);
    }
}
