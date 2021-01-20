using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMaker : MonoBehaviour
{
    Board board;
    int[] initInfo;
    public BoardMaker(Board board, int[] initInfo)
    {
        this.board = board;
        this.initInfo = initInfo;
    }

}
