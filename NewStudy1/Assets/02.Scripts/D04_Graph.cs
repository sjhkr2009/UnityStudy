using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D04_Graph : AlgorithmTest
{
    // 04. 그래프 1

    // 현실 세계의 사물이나 추상적인 개념 간의 연결 관계를 표현
    // 정점(Vertex) : 데이터를 표현(사물, 개념 등)
    // 간선(Edge) : 정점들을 연결하는데 사용


    // 종류
    // 가중치 그래프 : 간선에 추가로 값을 적어 가중치를 표기한 그래프 - 거리를 나타내어 지하철 노선도 등을 표현 가능
    // 방향 그래프 : 간선에 방향을 추가하여 연결 방향을 표기한 그래프 - 도로의 통행방향이나 인간관계등을 표현 가능
    // ...

    // 1. 그래프 생성 방식

    // 1) 인스턴스 생성 방식
    public override void TestCode01()
    {
        code1 = "인스턴스 생성 방식";

        //가장 단순하게는 다음과 같은 형태를 생각해볼 수 있다.

        List<D04_Vertex> vertex = new List<D04_Vertex>(6) { new D04_Vertex(), new D04_Vertex(), new D04_Vertex(), new D04_Vertex(), new D04_Vertex(), new D04_Vertex() }; //6개의 정점을 선언
        vertex[0].edges.Add(vertex[1]); //0번 정점과 1번 정점을 간선으로 연결
        vertex[0].edges.Add(vertex[3]);
        vertex[1].edges.Add(vertex[0]);
        vertex[1].edges.Add(vertex[2]);
        vertex[3].edges.Add(vertex[4]);

        // 직관적인 방법이지만, 매번 정점을 new Vertex로 선언해야 하며, 정점 자체는 추상적인 개념이라 당장은 정보가 없을수도 있는데 매번 생성하는게 낭비이기도 하다.
    }

    //-----------------------------------------------------------------------------------------------------

    // 2) 리스트 이용
    public override void TestCode02()
    {
        code2 = "리스트 이용 방식";

        //Vertex 인스턴스 생성 부담을 줄인 방식
        //읽는 방법: adjacent[from] -> 연결된 목록

        List<int>[] adjacent = new List<int>[6]
        {
            new List<int>{1,3}, //0번째 정점은 1,3번 정점과 연결되어 있다
            new List<int>{0,2,3},
            new List<int>{ },
            new List<int>{4},
            new List<int>{ },
            new List<int>{4}
        };

        //메모리는 아낄 수 있지만, 접근 속도에서 손해를 본다.
        //간선이 적고 정점이 많을 때 이점이 있다.

        //만일 가중치가 있는 그래프라면 다음과 같이 여러 값을 묶음(매핑)으로 저장할 수 있는 형태(튜플 등)로 저장하면 된다.

        List<D04_Edge>[] adjacent2 = new List<D04_Edge>[6]
        {
            new List<D04_Edge>{new D04_Edge(1,15), new D04_Edge(3,35)}, //0번째 정점은 1번 정점과 15의 가중치로, 3번 정점과 35의 가중치로 연결되어 있다
            new List<D04_Edge>{new D04_Edge(0,15), new D04_Edge(2,5), new D04_Edge(3,10)},
            new List<D04_Edge>{},
            new List<D04_Edge>{new D04_Edge(4,5)},
            new List<D04_Edge>{},
            new List<D04_Edge>{new D04_Edge(4,10)},
        };

        //하지만 1번과 마찬가지로 접근 속도가 느린 점은 여전하다.
        //특정 정점이 특정 정점과 연결되어 있냐는 여부를 알려면, 리스트를 순회하며 값을 하나씩 조회해봐야 한다.
    }

    //-----------------------------------------------------------------------------------------------------

    // 3) 행렬 이용하기 (2차원 배열)
    public override void TestCode03()
    {
        code3 = "2차원 배열(행렬) 이용 방식";

        //접근 속도를 높이기 위한 방식. 간선의 유무만 파악할 경우 bool로 만들수도 있다.
        int[,] adjacent = new int[6, 6]
        {
            { 0,1,0,1,0,0 },
            { 1,0,1,1,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,1,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,1,0 }
        };
        //메모리 소모가 심하지만(정점의 제곱만큼 값이 필요), 빠른 접근이 가능하다. 예를 들어 1번 정점과 3번이 연결되어 있는지 알고 싶으면 adjacent[1,3]만 확인하면 된다.
        //정점이 적고 간선이 많은 경우 이점이 있다.

        //가중치가 있는 경우, 값에 가중치를 입력하고 간선이 없을 경우 가중치로 쓰지 않는 수(여기서는 -1로 표기)로 표시하면 된다.
        int[,] adjacent2 = new int[6, 6]
        {
            { -1,15,-1,35,-1,-1 },
            { 15,-1,5,10,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,5,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-10,-1 }
        };
    }

    //이후 정점과 간선의 개수, 방향성과 가중치의 유무에 따라 (리스트나 행렬로) 구현을 달리할 수 있다. (정점이 많으면 리스트, 간선이 많으면 행렬 등)
    //리스트와 달리 그래프는 따로 구현해주는 클래스가 제공되지 않아 대부분 직접 값을 입력해서 구현해야 한다.

    //-----------------------------------------------------------------------------------------------------














    // 그래프 순회 방법

    // DFS (Depth First Search) : 깊이 우선 탐색
    // BFS (Breadth First Search) : 너비 우선 탐색

    // 여러 개의 방과 통로로 구현된 던전이 있을 때, 이를 도는 방법을 탐색한다고 생각해보자.
    // DFS는 무작정 직진하는 성향의 플레이어, BFS는 모든 방을 빠짐없이 탐색하려는 플레이어에 가깝다.


    int[,] adj1;
    List<int>[] adj2;
    void MakeGraph1()
    {
        //방향성이 없는 간선, 6개의 정점과 6개의 간선을 갖는 그래프를 생각해보자.
        //행렬과 리스트 두 형태로 모두 구현해본다. (둘은 같은 그래프)

        adj1 = new int[6, 6]
        {
            { 0,1,0,1,0,0 }, //방향성이 없는 간선이므로 대칭 형태가 된다. (1번 정점이 3번과 연결되면 3번도 1번과 연결된 것이므로)
            { 1,0,1,1,0,0 },
            { 0,1,0,0,0,0 },
            { 1,1,0,0,1,0 },
            { 0,0,0,1,0,1 },
            { 0,0,0,0,1,0 }
        };
        adj2 = new List<int>[]
        {
            new List<int>() {1,3},
            new List<int>() {0,2,3},
            new List<int>() {1},
            new List<int>() {0,1,4},
            new List<int>() {3,5},
            new List<int>() {4}
        };
    }








    // 2. DFS (깊이 우선 탐색)
    // DFS는 갈 수 있는 간선으로 무작정 가다가, 막다른 길에 다다르면 왔던 길을 돌아가며 다른 간선이 있는 정점을 찾는다.

    bool[] visited; //현재 정점을 탐색했는가의 여부
    public override void TestCode04()
    {
        code4 = "DFS(깊이 우선 탐색)에 따라 그래프 탐색 - 행렬 그래프";
        MakeGraph1();

        visited = new bool[6]; //방문 목록 초기화 후
        DFS(0); //시작점을 입력하고 실행
    }

    //정점을 입력하면 그와 이어져 있는 다른 정점을 방문하는 함수
    void DFS(int nowVertex)
    {
        Debug.Log($"{nowVertex}번째 정점 방문");
        visited[nowVertex] = true;

        //방문했음을 표시한 후, 다음 방문할 정점을 찾는다
        for (int nextVertex = 0; nextVertex < adj1.GetLength(0); nextVertex++)
        {
            //각 정점을 탐색해서, 간선이 없거나 이미 방문했다면 생략한다.
            if (adj1[nowVertex, nextVertex] == 0) continue;
            if (visited[nextVertex]) continue;

            //그 외의 정점을 찾으면 그곳을 방문하고, 다시 그곳에서 다른 정점을 탐색한다.
            DFS(nextVertex);

            //만일 그 다른 정점에서 방문하지 않은 정점을 못 찾았다면, DFS(nextVertex)의 for문이 끝나고 다시 이 정점 DFS(nowVertex)에서 다른 정점을 탐색할 것이다.
        }
    }

    //리스트 버전
    public override void TestCode05()
    {
        code5 = "DFS(깊이 우선 탐색)에 따라 그래프 탐색 - 리스트 그래프";
        MakeGraph1();

        visited = new bool[6];
        DFS2(0);
    }
    void DFS2(int now)
    {
        Debug.Log($"{now}번째 정점 방문");
        visited[now] = true;

        //행렬과 달리 간선이 있는 정점만 표기되어 있으므로, 간선 여부를 탐색할 필요는 없다.
        foreach (var next in adj2[now])
        {
            if (visited[next]) continue;
            DFS2(next);
        }
    }





    //하지만 모든 정점이 연결된 게 아니라면, 위의 방식으로 순회해도 방문하지 않은 정점이 있을 수 있다.
    //따라서 각 정점에서 방문 여부를 체크하는 것이 안전하다.
    void MakeGraph2()
    {
        // MakeGraph1 과 달리 3-4번의 간선을 끊는다.
        adj1 = new int[6, 6]
        {
            { 0,1,0,1,0,0 },
            { 1,0,1,1,0,0 },
            { 0,1,0,0,0,0 },
            { 1,1,0,0,0,0 },
            { 0,0,0,0,0,1 },
            { 0,0,0,0,1,0 }
        };
        adj2 = new List<int>[]
        {
            new List<int>() {1,3},
            new List<int>() {0,2,3},
            new List<int>() {1},
            new List<int>() {0,1},
            new List<int>() {5},
            new List<int>() {4}
        };
    }
    //각 정점에 대하여 DFS를 실행한다. DFS가 실행되는 순간 해당 정점과 연결된 정점은 모두 방문할 것이므로, 간선 없이 떨어져 있는 정점마다 1회씩만 실행될 것이다.
    void SearchAll()
    {
        for (int now = 0; now < adj1.GetLength(0); now++)
        {
            //모든 정점에 대해, 방문하지 않은 정점이면 DFS를 실행
            if (!visited[now]) DFS(now);
        }
    }
    public override void TestCode06()
    {
        code6 = "DFS - 간선이 끊겨 있는 정점들 방문하기";
        MakeGraph2();

        visited = new bool[6];
        SearchAll();
    }


    //-----------------------------------------------------------------------------------------------------










    // 3. BFS (너비 우선 탐색)
    // 각 정점을 방문할 때마다 그곳에서 갈 수 있는 다른 정점들을 탐색하여 대기 리스트에 넣고, 리스트에 가장 먼저 들어온 정점을 방문한다.
    // 선입선출을 이용하는 Queue 리스트를 활용한다.

    // DFS는 활용 여지가 다양한데, BFS는 대부분 길찾기(최단거리 찾기)에 이용된다.
    void BFS(int start)
    {
        Queue<int> q = new Queue<int>();
        bool[] found = new bool[6];

        q.Enqueue(start);
        found[start] = true;

        while (q.Count > 0)
        {
            int now = q.Dequeue();
            Debug.Log($"{now}번째 정점 방문");

            for (int next = 0; next < adj1.GetLength(0); next++) //리스트 그래프라면 경로의 존재여부(adj1[now, next] == 0)는 체크하지 않아도 된다.
            {
                if (adj1[now, next] == 0 || found[next]) continue;
                q.Enqueue(next);
                found[next] = true;
            }
        }
    }

    public override void TestCode07()
    {
        code7 = "BFS(너비 우선 탐색)에 따라 그래프 탐색 - 행렬 그래프";

        MakeGraph1();
        BFS(0);
    }



    // BFS에서는 이전에 방문한 정점이나 현재 정점에 도달하기까지의 거리 등을 추가로 탐색할 수 있다.
    void BFS2(int start)
    {
        Queue<int> q = new Queue<int>();
        bool[] found = new bool[6];
        q.Enqueue(start);
        found[start] = true;

        //추가된 부분: 어디로부터 방문했는지와 시작점에서 해당 정점에 방문하기까지의 거리를 표기할 공간을 만든다.
        int[] from = new int[6];
        int[] distance = new int[6];
        from[start] = start;
        distance[start] = 0;
        

        while (q.Count > 0)
        {
            int now = q.Dequeue();
            Debug.Log($"{now}번째 정점 방문 : {from[now]}로부터 방문함 / 시작점과의 거리 : {distance[now]}"); //추가된 부분도 디버그로 확인하자.

            for (int next = 0; next < adj1.GetLength(0); next++)
            {
                if (adj1[now, next] == 0 || found[next]) continue;
                q.Enqueue(next);
                found[next] = true;

                //추가된 부분:  이 정점에서 갈 수 있는 다른 정점들(next)은, 이곳으로부터 방문했다는 정보를 입력해준다. 이곳을 통해 방문하는 것이 최단경로가 될 것이다.
                //              또한 이것이 최단경로라면 next 정점은 반드시 이곳을 통해야 하므로, now보다 1 만큼 더 긴 거리를 갖는다.
                from[next] = now;
                distance[next] = distance[now] + 1;
            }
        }
    }

    public override void TestCode08()
    {
        code8 = "BFS - 거리와 경로 탐색";

        MakeGraph1();
        BFS2(0);
    }









    //------------------------------------------------------------------------------------



    // 4. 다익스트라 알고리즘(Dijkstra Algorithm)

    // BFS와 달리 가중치가 있는 그래프에 적용된다. 각 정점마다 간선과 가중치를 고려하여 최선의 탐색경로를 찾는다.
    // 가중치에 따라 탐색 경로를 수정해야 할 수 있으므로, Queue로 구현하지 않는다.

    void MakeGraph3()
    {
        // MakeGraph2 와 달리 각 간선에 가중치를 둔다.
        adj1 = new int[6, 6]
        {
            { 0,15,0,35,0,0 }, //간선이 없는 곳은 0으로 해도 되고, -1로 해도 된다. 나중에 경로를 찾을 때 알아볼 수 있게만 표시하면 됨.
            { 15,0,5,10,0,0 },
            { 0,5,0,0,0,0 },
            { 35,10,0,0,5,0 },
            { 0,0,0,5,0,5 },
            { 0,0,0,0,5,0 }
        };
    }
    
    void Dajikstra(int start)
    {
        bool[] visited = new bool[6];           //배열 생성 시 bool은 false로, int는 0으로 채워지게 된다.
        int[] distance = new int[6];            //거리가 0인지 방문하지 않아서 0인지 헷갈릴 수 있으니, 헷갈리지 않을만한 값으로 채워줄 필요가 있다.
        for (int i = 0; i < distance.Length; i++) distance[i] = int.MaxValue;   //distance의 기본값을 int의 최대값으로 넣어준다.

        int[] from = new int[6];
        from[start] = start;

        distance[start] = 0;

        while (true)
        {
            // 1) 아직 방문하지 않은 각 정점마다 출발점에서의 거리를 탐색해서, 가장 유력한 최단경로 후보를 찾는다.
            //    (거리를 모르는 곳은 int 최대값을 넣어줬으니, 처음엔 유일하게 거리가 0인 출발점부터 방문할 것이다)
            int minDistance = int.MaxValue;
            int now = -1;

            for (int vertex = 0; vertex < adj1.GetLength(0); vertex++)
            {
                if (visited[vertex]) continue;
                if (distance[vertex] == int.MaxValue || distance[vertex] >= minDistance) continue;

                minDistance = distance[vertex];
                now = vertex;
            }

            // 2) 해당 후보를 방문한다. 방문할 곳을 찾지 못했다면 함수를 종료한다.
            if (now == -1) break;
            visited[now] = true;
            Debug.Log($"{now}번째 정점 방문 (이동거리 : {distance[now]})");


            // 3) 방문한 정점에 연결된 간선을 조사해서, 그 간선을 통해 인접한 다른 정점으로 가는 게 (지금껏 발견한 거리보다) 최단거리일 경우 최단거리를 갱신한다.
            for (int next = 0; next < adj1.GetLength(1); next++)
            {
                if (adj1[now, next] == 0) continue;
                if (visited[next]) continue;

                int distanceThroughNow = distance[now] + adj1[now, next];
                if (distanceThroughNow < distance[next])
                {
                    string _string = distance[next] == int.MaxValue ? "알 수 없음" : distance[next].ToString(); //로그 출력을 위한 부분

                    distance[next] = distanceThroughNow;
                    from[next] = now;

                    Debug.Log($"{now}번째 정점에서 {next}번째로 이동하도록 경로가 변경됩니다. (갱신된 이동거리 : {_string} → {distanceThroughNow})");
                }
            }
        }
        for (int vertex = 0; vertex < adj1.GetLength(0); vertex++)
        {
            Debug.Log($"{vertex}번째 정점까지의 최단 거리 : {distance[vertex]} / 이전 정점 : {from[vertex]}번");
        }
    }

    public override void TestCode09()
    {
        code9 = "Dajikstra에 따라 그래프 탐색 - 행렬 그래프";

        MakeGraph3();
        Dajikstra(0);
    }
}

class D04_Vertex
{
    public List<D04_Vertex> edges = new List<D04_Vertex>();
}
class D04_Edge
{
    public int vertex;
    public int weight;
    public D04_Edge(int v, int w) { vertex = v; weight = w; }
}
