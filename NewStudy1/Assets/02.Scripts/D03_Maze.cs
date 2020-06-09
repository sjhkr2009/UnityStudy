using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class D03_Maze : AlgorithmTest
{
    public enum TileType
    {
        Empty,
        Wall
    }
    
    public int mapSize = 25;
    public Text text;
    TileType[,] tiles;


    public override void TestCode01()
    {
        code1 = "미로 생성 - 외곽만 벽으로";

        tiles = new TileType[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (x == 0 || x == mapSize - 1 || y == 0 || y == mapSize - 1)
                    tiles[y, x] = TileType.Wall;
                else
                    tiles[y, x] = TileType.Empty;
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

        tiles = new TileType[mapSize, mapSize];

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
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = TileType.Wall;
                else tiles[y, x] = TileType.Empty;
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
                    tiles[y + 1, x] = TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = TileType.Empty;
                    continue;
                }

                //그 외 일반적인 빈 공간은 우측이나 아래쪽을 랜덤으로 뚫어준다.
                if (Random.value <= 0.5f) tiles[y, x + 1] = TileType.Empty;
                else tiles[y + 1, x] = TileType.Empty;
            }
        }

        CreateMaze(tiles);
    }

    public override void TestCode03()
    {
        code3 = "미로 생성 - SideWinder 알고리즘";

        tiles = new TileType[mapSize, mapSize];

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
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = TileType.Wall;
                else tiles[y, x] = TileType.Empty;
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
                    tiles[y + 1, x] = TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = TileType.Empty;
                    continue;
                }
                //여기까지는 동일.

                //가로로 뚫을 경우 카운트를 1씩 증가시키고, 세로로 뚫을 때 지금까지 가로로 뚫어온 부분들 중 하나를 선택해서 뚫는다.
                if (Random.value <= 0.5f)
                {
                    tiles[y, x + 1] = TileType.Empty;
                    count++;
                }
                else
                {
                    int randomIndex = Random.Range(0, count); //0이면 현재 지점에서, 그 이상이면 해당 숫자만큼 이전 지점으로 가서 뚫는다.
                    tiles[y + 1, x - randomIndex * 2] = TileType.Empty; //원래 벽이었던 곳은 세로로 뚫으면 막힌 곳이므로, 원래 비어있던 곳만 선택한다. 즉 인덱스는 2칸 단위로 움직인다.
                    count = 1; //카운트는 초기화한다.
                }
            }
        }
        CreateMaze(tiles);
    }

    /// <summary>
    /// 우측 상단에서 좌측 하단 순서로 미로를 생성한다.
    /// </summary>
    void CreateMaze(TileType[,] logic)
    {
        if (text == null) text = FindObjectOfType<Text>();

        text.text = "";
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                text.text += TileColor(logic[y, x]);
            }
            text.text += "\n";
        }
    }

    string TileColor(TileType type)
    {
        switch (type)
        {
            case TileType.Empty:
                return "<color=#00ff00ff>●</color>"; //초록
            case TileType.Wall:
                return "<color=#ff0000ff>●</color>"; //빨강
            default:
                return "<color=#00ff00ff>●</color>";
        }
    }
}
