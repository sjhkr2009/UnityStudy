using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum E03_TileType
{
    Empty,
    Wall
}

//미로 생성 및 길찾기

public class E03_Maze : AlgorithmTest
{
    
    public int mapSize = 25;
    public Text text;
    E03_TileType[,] tiles;
    E03_Player player = new E03_Player();

    const string playerColor = "<color=#ffffffff>●</color>"; //흰색
    const string emptyColor = "<color=#008000ff>●</color>"; //초록
    const string wallColor = "<color=#ff0000ff>●</color>"; //빨강
    const string goalColor = "<color=#ffff00ff>●</color>"; //노랑

    public override void TestCode01()
    {
        code1 = "미로 생성 - 외곽만 벽으로";

        player.Init();
        tiles = new E03_TileType[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (x == 0 || x == mapSize - 1 || y == 0 || y == mapSize - 1)
                    tiles[y, x] = E03_TileType.Wall;
                else
                    tiles[y, x] = E03_TileType.Empty;
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

        player.Init();
        tiles = new E03_TileType[mapSize, mapSize];

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
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = E03_TileType.Wall;
                else tiles[y, x] = E03_TileType.Empty;
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
                    tiles[y + 1, x] = E03_TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = E03_TileType.Empty;
                    continue;
                }

                //그 외 일반적인 빈 공간은 우측이나 아래쪽을 랜덤으로 뚫어준다.
                if (UnityEngine.Random.value <= 0.5f) tiles[y, x + 1] = E03_TileType.Empty;
                else tiles[y + 1, x] = E03_TileType.Empty;
            }
        }

        CreateMaze(tiles);
    }

    /// <summary>
    /// SideWinder 미로 생성 알고리즘
    /// </summary>
    public override void TestCode03()
    {
        code3 = "미로 생성 - SideWinder 알고리즘";

        player.Init();
        tiles = new E03_TileType[mapSize, mapSize];

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
                if (x % 2 == 0 || y % 2 == 0) tiles[y, x] = E03_TileType.Wall;
                else tiles[y, x] = E03_TileType.Empty;
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
                    tiles[y + 1, x] = E03_TileType.Empty;
                    continue;
                }
                if (y == mapSize - 2)
                {
                    tiles[y, x + 1] = E03_TileType.Empty;
                    continue;
                }
                //여기까지는 동일.

                //가로로 뚫을 경우 카운트를 1씩 증가시키고, 세로로 뚫을 때 지금까지 가로로 뚫어온 부분들 중 하나를 선택해서 뚫는다.
                if (UnityEngine.Random.value <= 0.5f)
                {
                    tiles[y, x + 1] = E03_TileType.Empty;
                    count++;
                }
                else
                {
                    int randomIndex = UnityEngine.Random.Range(0, count); //0이면 현재 지점에서, 그 이상이면 해당 숫자만큼 이전 지점으로 가서 뚫는다.
                    tiles[y + 1, x - randomIndex * 2] = E03_TileType.Empty; //원래 벽이었던 곳은 세로로 뚫으면 막힌 곳이므로, 원래 비어있던 곳만 선택한다. 즉 인덱스는 2칸 단위로 움직인다.
                    count = 1; //카운트는 초기화한다.
                }
            }
        }
        CreateMaze(tiles);
    }

    //----------------------------------------------------------------------------------------------------------------








    // 우수법으로 길찾기

    public override void TestCode04()
    {
        code4 = "오른손 법칙에 따라 길찾기 (플레이 중에만 적용, 미로 생성 후 사용할 것)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(StartRightHandGame));
    }

    public override void TestCode05()
    {
        code5 = "우수법 반복 재실행 (플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(RH_AutoRestart));
    }

    [Button("코루틴 중지")]
    public void Stop() { StopAllCoroutines(); }

    IEnumerator StartRightHandGame()
    {
        player.Init();
        while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
        {
            player.MoveByRightHandLogic(tiles);
            CreateMaze(tiles);

            yield return new WaitForSeconds(0.07f);
        }
        Debug.Log("도착");
    }
    IEnumerator RH_AutoRestart()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            player.Init();
            TestCode03();

            while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
            {
                player.MoveByRightHandLogic(tiles);
                CreateMaze(tiles);

                yield return new WaitForSeconds(0.03f);
            }
        }
    }

    /// <summary>
    /// 우측 상단에서 좌측 하단 순서로 미로를 생성한다. 로직을 만든 후에 실행할 것.
    /// </summary>
    void CreateMaze(E03_TileType[,] logic)
    {
        if (text == null) text = FindObjectOfType<Text>();

        text.text = "";
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (y == player.PosY && x == player.PosX) text.text += playerColor;
                else if (y == mapSize - 2 && x == mapSize - 2) text.text += goalColor;
                else text.text += TileColor(logic[y, x]);
            }
            text.text += "\n";
        }
    }

    string TileColor(E03_TileType type)
    {
        switch (type)
        {
            case E03_TileType.Empty:
                return emptyColor;
            case E03_TileType.Wall:
                return wallColor; 
            default:
                return emptyColor;
        }
    }

    //----------------------------------------------------------------------------------------------------------------









    // BFS 길찾기
    // 설명은 D04의 해당 부분 참고

    IEnumerator StartBFSGame()
    {
        player.MakeBFSLogic(tiles);
        player.Init();
        int count = 0;

        while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
        {
            player.MoveByBFSLogic(count);
            CreateMaze(tiles);
            count++;

            yield return new WaitForSeconds(0.07f);
        }
        Debug.Log("도착");
    }
    IEnumerator BFS_AutoRestart()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            player.Init();
            TestCode03();

            player.MakeBFSLogic(tiles);
            int count = 0;

            while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
            {
                player.MoveByBFSLogic(count);
                CreateMaze(tiles);
                count++;

                yield return new WaitForSeconds(0.03f);
            }
        }
    }

    public override void TestCode06()
    {
        code6 = "BFS에 따른 길찾기 실행(플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(StartBFSGame));
    }

    public override void TestCode07()
    {
        code7 = "BFS 반복 재실행(플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(BFS_AutoRestart));
    }

    //BFS 길찾기의 단점
    // 1) 불필요한 경로까지 모두 탐색하여 비용이 많이 든다
    // 2) 제한적인 상황에서만 사용할 수 있다 : 상하좌우 이동, 1칸씩 이동, 가중치 없음 (모든 경로로의 이동 비용 동일)

    //  -> 이는 다익스트라로 가중치를 부여하여 개선할 수 있다.

    //----------------------------------------------------------------------------------------------------------------








    // A* (에이스타) 길찾기

    // BFS (또는 다익스트라)는 시작점만 알고 목적지를 모르는 알고리즘이므로, 모든 경로를 탐색하여 목적지가 나올 때까지 찾는다.
    // 에이스타 알고리즘은 목적지를 아는 상태에서, 목적지에 가까워질수록 가산점을 부여하여 직관성을 높이고 불필요한 경로의 탐색을 최소화한다.

    // 점수 매기기 방식
    // 1. 시작점에서 해당 좌표까지의 거리 (플레이어가 온 길에 따라 가변적)
    // 2. 해당 좌표에서 목적지까지의 거리 (각 좌표마다 고정적으로 부여된 값)
    // 최종 점수 = 1 + 2 (낮을수록 좋음)

    IEnumerator StartAStarGame()
    {
        player.Init();
        player.MakeAStarLogic(tiles);
        int index = 0;

        while(player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
        {
            player.MoveByAStarLogic(index);
            CreateMaze(tiles);
            index++;

            yield return new WaitForSeconds(0.07f);
        }
        Debug.Log("도착");
    }
    IEnumerator AStar_AutoRestart()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            player.Init();
            TestCode03();

            player.MakeBFSLogic(tiles);
            int index = 0;

            while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
            {
                player.MoveByAStarLogic(index);
                CreateMaze(tiles);
                index++;

                yield return new WaitForSeconds(0.035f);
            }
            Debug.Log("도착");
        }
    }

    public override void TestCode08()
    {
        code8 = "AStar에 따른 길찾기 실행(플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(StartAStarGame));
    }

    public override void TestCode09()
    {
        code9 = "AStar 반복 재실행(플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(AStar_AutoRestart));
    }



    // A* 길찾기 - 대각선 이동 가능
    IEnumerator AStarGame_canDiagonalMove()
    {
        player.Init();
        TestCode03();
        player.MakeAStarLogic2(tiles);
        int index = 0;

        while (player.PosY != mapSize - 2 || player.PosX != mapSize - 2)
        {
            player.MoveByAStarLogic(index);
            CreateMaze(tiles);
            index++;

            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("도착");
    }

    public override void TestCode10()
    {
        code9 = "AStar 대각선 이동 플레이 (플레이 중에만 적용)";
        if (!doOneTime) doOneTime = true;
        StartCoroutine(nameof(AStarGame_canDiagonalMove));
    }

}




class E03_Player
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

    public void Init()
    {
        PosX = 1;
        PosY = 1;
        dir = (int)Direction.Down;
    }

    public E03_Player()
    {
        PosX = 1;
        PosY = 1;
        dir = (int)Direction.Down;
    }


    //오른손 법칙(우수법) : 미로에서 벽에 오른손을 대고 길을 찾는방식
    public void MoveByRightHandLogic(E03_TileType[,] maze)
    {
        //1. 오른쪽으로 갈 수 있으면 우회전 후 앞으로 1칸
        if (maze[PosY + RightY, PosX + RightX] == E03_TileType.Empty)
        {
            dir = (dir + 1) % 4;
            PosX += ForwardX;
            PosY += ForwardY;
        }
        //2. 앞으로 갈 수 있으면 앞으로 1칸
        else if (maze[PosY + ForwardY, PosX + ForwardX] == E03_TileType.Empty)
        {
            PosX += ForwardX;
            PosY += ForwardY;
        }
        //3. 둘 다 갈 수 없으면 좌회전
        else
        {
            dir = (dir + 3) % 4;
        }
    }


    //---------------------------------------------------------------------------------------




    //BFS 길찾기에서 이동경로를 저장할 리스트

    List<E03_Pos> BFSRoad = new List<E03_Pos>();

    //---------------------------------------------------------------------------------------


    //그래프를 이용한 길찾기 - BFS(너비 우선 탐색) 방식을 이용하여 길찾기
    public void MakeBFSLogic(E03_TileType[,] maze)
    {
        int mapSize = maze.GetLength(0);
        
        bool[,] found = new bool[mapSize, mapSize];
        Queue<E03_Pos> q = new Queue<E03_Pos>();

        int startY = 1;
        int startX = 1;

        q.Enqueue(new E03_Pos(startY, startX));
        found[startY, startX] = true;

        E03_Pos[,] from = new E03_Pos[mapSize, mapSize];
        from[startY, startX] = new E03_Pos(startY, startX);

        while (q.Count > 0)
        {
            E03_Pos pos = q.Dequeue();
            int nowY = pos.Y;
            int nowX = pos.X;

            for (int i = 0; i < forwardX.Length; i++)
            {
                int nextPosY = nowY + forwardY[i];
                int nextPosX = nowX + forwardX[i];

                if (nextPosY < 0 || nextPosY >= mapSize || nextPosX < 0 || nextPosX >= mapSize) continue;
                if (found[nextPosY, nextPosX]) continue;
                if (maze[nextPosY, nextPosX] == E03_TileType.Wall) continue;

                q.Enqueue(new E03_Pos(nextPosY, nextPosX));
                found[nextPosY, nextPosX] = true;

                from[nextPosY, nextPosX] = new E03_Pos(nowY, nowX);
            }
        }

        BFSRoad = MakeRoad(from, mapSize);
    }

    List<E03_Pos> MakeRoad(E03_Pos[,] from, int mapSize)
    {
        int goalY = mapSize - 2;
        int goalX = mapSize - 2;

        List<E03_Pos> road = new List<E03_Pos>();
        E03_Pos nowPos = new E03_Pos(goalY, goalX);

        while (true)
        {
            road.Add(nowPos);
            if (nowPos == from[nowPos.Y, nowPos.X]) break;
            nowPos = from[nowPos.Y, nowPos.X];
        }
        road.Reverse();

        return road;
    }

    public void MoveByBFSLogic(int index)
    {
        if(BFSRoad[index] == null)
        {
            Debug.Log("로직이 생성되지 않았거나 이동 범위를 이탈했습니다.");
            return;
        }

        PosY = BFSRoad[index].Y;
        PosX = BFSRoad[index].X;
    }

    //---------------------------------------------------------------------------------------




    //A* 길찾기에서 이동경로를 저장할 리스트
    List<E03_Pos> AStarRoad = new List<E03_Pos>();

    //이동 비용. 현재는 상하좌우 모두 1로 둔다.
    int[] moveCost = new int[] { 1, 1, 1, 1 };

    //---------------------------------------------------------------------------------------

    public void MakeAStarLogic(E03_TileType[,] maze)
    {
        int mapSize = maze.GetLength(0);
        int goalY = mapSize - 2;
        int goalX = mapSize - 2;
        int startY = PosY;
        int startX = PosX;

        bool[,] closed = new bool[mapSize, mapSize];
        int[,] open = new int[mapSize, mapSize];
        E03_Pos[,] from = new E03_Pos[mapSize, mapSize];
        from[startY, startX] = new E03_Pos(startY, startX);

        for (int y = 0; y < open.GetLength(0); y++)
            for (int x = 0; x < open.GetLength(1); x++)
                open[y, x] = int.MaxValue;

        D05_PriorityQueue<E03_PQNode> pq = new D05_PriorityQueue<E03_PQNode>();

        open[startY, startY] = Mathf.Abs(goalY - startY) + Math.Abs(goalX - startX) + 0; // 시작점이므로 현재까지 온 거리는 0이다. 사실 더해주지 않아도 무방하다.
        pq.Push(new E03_PQNode() { F = Mathf.Abs(goalY - startY) + Math.Abs(goalX - startX), G = 0, X = startX, Y = startY });

        while(pq.Count > 0)
        {
            E03_PQNode node = pq.Pop();

            if (closed[node.Y, node.X]) continue;

            closed[node.Y, node.X] = true;
            int nowY = node.Y;
            int nowX = node.X;

            if (nowY == goalY && nowX == goalX) break;

            for (int i = 0; i < forwardX.Length; i++)
            {
                int nextY = nowY + forwardY[i];
                int nextX = nowX + forwardX[i];

                if (nextX < 0 || nextY < 0 || nextX >= mapSize || nextY >= mapSize) continue;
                if (closed[nextY, nextX]) continue;
                if (maze[nextY, nextX] == E03_TileType.Wall) continue;

                int g = node.G + moveCost[i];
                int h = Mathf.Abs(goalY - nowY) + Math.Abs(goalX - nowX);
                int distance = g + h;

                if (open[nextY, nextX] < distance) continue;

                pq.Push(new E03_PQNode() { F = distance, G = g, X = nextX, Y = nextY });
                open[nextY, nextX] = distance;
                from[nextY, nextX] = new E03_Pos(nowY, nowX);
            }
        }

        AStarRoad = MakeRoad(from, mapSize);
    }

    public void MoveByAStarLogic(int index)
    {
        if (AStarRoad[index] == null)
        {
            Debug.Log("로직이 생성되지 않았거나 이동 범위를 이탈했습니다.");
            return;
        }

        PosY = AStarRoad[index].Y;
        PosX = AStarRoad[index].X;
    }


    //---------------------------------------------------------------------------------------





    // A* 로 대각선 이동 구현하기

    //이동 비용 및 이동방향에 따른 좌표값 변화. 순서대로 상/하/좌/우/좌상/우상/좌하/우하 순서로 입력한다.
    //이동 비용을 1칸당 10으로 간주한다.
    int[] moveCost2 = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };
    int[] forwardX2 = new int[] { 0, 0, -1, 1, -1, 1, -1, 1 };
    int[] forwardY2 = new int[] { -1, 1, 0, 0, -1, 1, -1, 1 };

    //---------------------------------------------------------------------------------------

    public void MakeAStarLogic2(E03_TileType[,] maze)
    {
        int mapSize = maze.GetLength(0);
        int goalY = mapSize - 2;
        int goalX = mapSize - 2;

        bool[,] closed = new bool[mapSize, mapSize];
        int[,] open = new int[mapSize, mapSize];
        for (int y = 0; y < mapSize; y++)
            for (int x = 0; x < mapSize; x++)
                open[y, x] = int.MaxValue;

        E03_Pos[,] from = new E03_Pos[mapSize, mapSize];
        from[PosY, PosX] = new E03_Pos(PosY, PosX);

        D05_PriorityQueue<E03_PQNode> pq = new D05_PriorityQueue<E03_PQNode>();

        open[PosY, PosX] = 10 * (Mathf.Abs(goalY - PosY) + Mathf.Abs(goalX - PosX));
        pq.Push(new E03_PQNode() { F = open[PosY, PosX], G = 0, X = PosX, Y = PosY });

        while(pq.Count > 0)
        {
            E03_PQNode node = pq.Pop();
            int nowY = node.Y;
            int nowX = node.X;

            if (closed[nowY, nowX]) continue;
            if (nowY == goalY && nowX == goalX) break;

            for (int i = 0; i < forwardX2.Length; i++)
            {
                int nextY = nowY + forwardY2[i];
                int nextX = nowX + forwardX2[i];

                if (nextX < 0 || nextY < 0 || nextX >= mapSize || nextY >= mapSize) continue;
                if (maze[nextY, nextX] == E03_TileType.Wall) continue;
                if (closed[nextY, nextX]) continue;

                int g = node.G + moveCost2[i];
                int h = 10 * (Mathf.Abs(goalY - nextY) + Mathf.Abs(goalX - nextX));

                if (open[nextY, nextX] < g + h) continue;

                open[nextY, nextX] = g + h;
                pq.Push(new E03_PQNode() { F = g + h, G = g, X = nextX, Y = nextY });
                from[nextY, nextX] = new E03_Pos(nowY, nowX);
            }
        }

        AStarRoad = MakeRoad(from, mapSize);
    }
}





/// <summary>
/// 좌표값을 저장할 클래스
/// </summary>
class E03_Pos
{
    public E03_Pos(int y, int x)
    {
        Y = y;
        X = x;
    }
    public int Y;
    public int X;
}


/// <summary>
/// A*에서 우선순위 큐의 노드의 역할을 할 클래스
/// </summary>
class E03_PQNode : IComparable<E03_PQNode>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int F { get; set; } // 최종 점수 (낮을수록 좋음)
    public int G { get; set; } // 현재 좌표까지 이동한 거리

    public int CompareTo(E03_PQNode other)
    {
        if (F == other.F) return 0;
        else if (F > other.F) return -1; // 낮을수록 좋은 값이므로, 현재값이 비교대상보다 크면 -1(=안 좋음)을 반환한다.
        else return 1;
    }
}