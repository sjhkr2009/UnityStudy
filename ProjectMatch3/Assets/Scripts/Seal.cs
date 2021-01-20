using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// Cell에 고정되어 있는 장애물 요소입니다. 해제 전까지 블록의 이동이나 파괴가 제한될 수 있습니다.
/// (현재는 사용되지 않습니다)
/// </summary>
public class Seal : MonoBehaviour
{
    public int sealType;
    public int hp;

    public bool IsSealed => (sealType & (int)SealType.Immovable) > 0 && (hp > 0);
}
