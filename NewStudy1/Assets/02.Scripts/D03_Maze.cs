using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public enum D03_TileType
{
    Empty,
    Wall
}

public class D03_Maze : AlgorithmTest
{
    
    public int mapSize = 25;
    public Text text;
    D03_TileType[,] tiles;
    D03_Player player = new D03_Player();


    public override void TestCode01()
    {
        code1 = "미로 생성 - 외곽만 벽으로";

        tiles = new D03_TileType[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (x == 0 || x == mapSize - 1 || y == 0 || y == mapSize - 1)
                    tiles[y, x] = D03_TileType.Wall;
                else
                    tiles[y, x] = D03_TileType.Empty;
            }
        }
        CreateMaze(tiles);
    }

    /// <summary>
    /// Binary Tree 미로 생성 알고리즘
    /// </summary>
    public override void TestCode02()
    {
        code2 = "미로 생성 - Binary Tree 알고리즘";

        tiles = new D03_TileType[mapSize, mapSize];

        //x나 y가 짝수인 경우 전부 벽으로 처리하면, 창문과 비슷한 형태가 된다.
        //이 로직에서 맵 외곽이 벽으로 둘러싸이려면 반드시 홀수 x 홀수 개의 형태가 되어야 하므로, 짝수 사이즈 입력 시 리턴한다.
        if (mapSize % 2 == 0)
        {
            Debug.Log("Map Size는 홀수여야 합니다.");
            return;
        }
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = D03_TileType.Wall;
                else tiles[y, x] = D03_TileType.Empty;
            }
        }

        //비어 있는 칸의 우측 또는 아래쪽 중 하나를 랜덤으로 뚫어준다.
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                //벽에는 적용하지 않는다. 참고로 여기서 조건을 tiles[y, x] == TileType.Wall 과 같이 하면, 뚫어준 칸에서 또 함수가 발동해서 예상보다 많은 칸이 Empty가 되어버린다.
                if (x % 2 == 0 || y % 2 == 0) continue;

                //외곽은 벽이어야 하므로, 외곽 바로 앞의 빈 공간에서 외곽 벽을 없애지 않도록 한다.
                //맨 우측 하단의 빈칸은 더 이상 뚫지 않도록 continue 시킨다.
                if (x == mapSize - 2 && y == mapSize - 2) continue;
                if(x == mapSize - 2)
                {
                    tiles[y + 1, x] = D03_TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = D03_TileType.Empty;
                    continue;
                }

                //그 외 일반적인 빈 공간은 우측이나 아래쪽을 랜덤으로 뚫어준다.
                if (Random.value <= 0.5f) tiles[y, x + 1] = D03_TileType.Empty;
                else tiles[y + 1, x] = D03_TileType.Empty;
            }
        }

        CreateMaze(tiles);
    }

    public override void TestCode03()
    {
        code3 = "미로 생성 - SideWinder 알고리즘";

        tiles = new D03_TileType[mapSize, mapSize];

        //x나 y가 짝수인 경우 전부 벽으로 처리하는 것까지는 Binary Tree와 동일하다.
        if (mapSize % 2 == 0)
        {
            Debug.Log("Map Size는 홀수여야 합니다.");
            return;
        }
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = D03_TileType.Wall;
                else tiles[y, x] = D03_TileType.Empty;
            }
        }

        //아래쪽으로 길을 뚫을 때 반드시 빈 타일에서 뚫는 것이 아니라, 같은 x축 상의 이전 지점들 중 한 지점에서 길을 뚫는다.
        for (int y = 0; y < mapSize; y++)
        {
            int count = 1; //가로로 움직인 횟수를 체크한 카운트 생성

            for (int x = 0; x < mapSize; x++)
            {
                if (x % 2 == 0 || y % 2 == 0) continue;

                if (x == mapSize - 2 && y == mapSize - 2) continue;
                if (x == mapSize - 2)
                {
                    tiles[y + 1, x] = D03_TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = D03_TileType.Empty;
                    continue;
                }
                //여기까지는 동일.

                //가로로 뚫을 경우 카운트를 1씩 증가시키고, 세로로 뚫을 때 지금까지 가로로 뚫어온 부분들 중 하나를 선택해서 뚫는다.
                if (Random.value <= 0.5f)
                {
                    tiles[y, x + 1] = D03_TileType.Empty;
                    count++;
                }
                else
                {
                    int randomIndex = Random.Range(0, count); //0이면 현재 지점에서, 그 이상이면 해당 숫자만큼 이전 지점으로 가서 뚫는다.
                    tiles[y + 1, x - randomIndex * 2] = D03_TileType.Empty; //원래 벽이었던 곳은 세로로 뚫으면 막힌 곳이므로, 원래 비어있던 곳만 선택한다. 즉 인덱스는 2칸 단위로 움직인다.
                    count = 1; //카운트는 초기화한다.
                }
            }
        }
        CreateMaze(tiles);
    }

    public override void TestCode04()
    {
        code4 = "오른손 법칙에 따라 길찾기 (플레이 중에만 적용)";
        StopCoroutine(nameof(StartRightHandGame));
        StartCoroutine(nameof(StartRightHandGame));
    }

    IEnumerator StartRightHandGame()
    {
        while(player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
        {
            PlayerMoveToRightHandLogic();
            CreateMaze(tiles);

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("도착");
    }

    /// <summary>
    /// 우측 상단에서 좌측 하단 순서로 미로를 생성한다.
    /// </summary>
    void CreateMaze(D03_TileType[,] logic)
    {
        if (text == null) text = FindObjectOfType<Text>();

        text.text = "";
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (y == player.PosY && x == player.PosX) text.text += "<color=#00ffffff>●</color>"; //청록
                else if (y == mapSize - 2 && x == mapSize - 2) text.text += "<color=#ffff00ff>●</color>"; //노랑
                else text.text += TileColor(logic[y, x]);
            }
            text.text += "\n";
        }
    }

    string TileColor(D03_TileType type)
    {
        switch (type)
        {
            case D03_TileType.Empty:
                return "<color=#00ff00ff>●</color>"; //초록
            case D03_TileType.Wall:
                return "<color=#ff0000ff>●</color>"; //빨강
            default:
                return "<color=#00ff00ff>●</color>";
        }
    }

    //오른손 법칙 : 미로에서 벽에 오른손을 대고 길을 찾는방식
    public void PlayerMoveToRightHandLogic()
    {
        //1. 오른쪽으로 갈 수 있으면 우회전 후 앞으로 1칸
        if (tiles[player.PosY + player.RightY, player.PosX + player.RightX] == D03_TileType.Empty)
        {
            player.dir = (player.dir + 4) % 4;
            player.PosX += player.ForwardX;
            player.PosY += player.ForwardY;
        }

        //2. 앞으로 갈 수 있으면 앞으로 1칸
        else if (tiles[player.PosY + player.ForwardY, player.PosX + player.ForwardX] == D03_TileType.Empty)
        {
            player.PosX += player.ForwardX;
            player.PosY += player.ForwardY;
        }

        //3. 둘 다 갈 수 없으면 우회전
        else
        {
            player.dir = (player.dir + 4) % 4;
        }
    }
}

class D03_Player
{
    public int PosX { get; set; }
    public int PosY { get; set; }

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public int dir;
    int[] rightX = { 1, 0, -1, 0 };
    int[] rightY = { 0, 1, 0, -1 };
    int[] forwardX = { 0, 1, 0, -1 };
    int[] forwardY = { -1, 0, 1, 0 };

    public int RightX => rightX[dir];
    public int RightY => rightY[dir];
    public int ForwardX => forwardX[dir];
    public int ForwardY => forwardY[dir];

    public D03_Player()
    {
        PosX = 1;
        PosY = 1;
        dir = (int)Direction.Down;
    }
}