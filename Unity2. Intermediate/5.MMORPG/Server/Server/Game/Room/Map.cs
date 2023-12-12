namespace Server.Game; 

using System;
using System.Collections.Generic;
using System.IO;
using Utility;

public class Map {
	public int MinX { get; internal set; }
	public int MaxX { get; internal set; }
	public int MinY { get; internal set; }
	public int MaxY { get; internal set; }
	public int SizeX => MaxX - MinX + 1;
	public int SizeY => MaxY - MinY + 1;

	private bool[,] collisionData;
	private Player[,] players;

	private static string GetMapName(int id) => $"Map_{id:000}";
	private const string DefaultPathPrefix = "../../../../../Imported/MapData";
	
	public void LoadMap(int mapId, string pathPrefix = DefaultPathPrefix) {
		string mapName = GetMapName(mapId);
		var collisionDataFile = File.ReadAllText($"{pathPrefix}/{mapName}.txt");
		
		SetCollisionData(collisionDataFile);
	}

	private void SetCollisionData(string collisionInfoText) {
		StringReader reader = new StringReader(collisionInfoText);

		MinX = int.Parse(reader.ReadLine());
		MaxX = int.Parse(reader.ReadLine());
		MinY = int.Parse(reader.ReadLine());
		MaxY = int.Parse(reader.ReadLine()) + 1; // MaxY에서 1을 뺐으니 다시 더해준다. 어떤 이슈인지는 MapEditor.GenerateMapInfo 참고.

		int yCount = MaxY - MinY + 1;
		int xCount = MaxX - MinX + 1;
		collisionData = new bool[yCount, xCount];
		players = new Player[yCount, xCount];

		for (int y = 0; y < yCount; y++) {
			string line = reader.ReadLine();
			if (line == null) break;

			for (int x = 0; x < xCount; x++) {
				try {
					collisionData[y, x] = line[x] != '0';
				}
				catch (IndexOutOfRangeException e) {
					Console.WriteLine($"({x},{y}) 좌표를 찾을 수 없습니다 / Max:{MaxX},{MaxY} / Min: {MinX},{MinY} / Count: {xCount},{yCount} / Line: {line.Length} \n에러 메시지: {e}");
				}
			}
		}
	}

	public bool CanGo(Vector2Int cellPos, bool checkPlayers = true) {
		// 맵 범위 밖이면 false
		if (cellPos.x < MinX || cellPos.x > MaxX || cellPos.y < MinY || cellPos.y > MaxY) return false;

		// 맵 배열 내에서의 인덱스를 구한다. 맵 왼쪽 위를 원점으로 간주한다. 
		int x = cellPos.x - MinX;
		int y = MaxY - cellPos.y;

		bool existCollision = collisionData[y, x];
		bool blockByPlayer = checkPlayers && players[y, x] != null; 
		return !existCollision && !blockByPlayer;
	}

	#region A* PathFinding

	// U D L R
	int[] _deltaY = new int[] { 1, -1, 0, 0 };
	int[] _deltaX = new int[] { 0, 0, -1, 1 };
	int[] _cost = new int[] { 10, 10, 10, 10 };

	public List<Vector2Int> FindPath(Vector2Int startCellPos, Vector2Int destCellPos, bool ignoreDestCollision = false) {
		// (y, x) 이미 방문했는지 여부 (방문 = closed 상태)
		bool[,] closed = new bool[SizeY, SizeX]; // CloseList

		// (y, x) 가는 길을 한 번이라도 발견했는지
		// 발견X => MaxValue
		// 발견O => Priority (= cost + dist)
		int[,] open = new int[SizeY, SizeX]; // OpenList
		for (int y = 0; y < SizeY; y++)
		for (int x = 0; x < SizeX; x++)
			open[y, x] = Int32.MaxValue;

		ZeroBasedPos[,] parent = new ZeroBasedPos[SizeY, SizeX];

		// 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
		PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

		// CellPos -> ArrayPos
		ZeroBasedPos zeroBasedPos = Cell2ZeroBasedPos(startCellPos);
		ZeroBasedPos dest = Cell2ZeroBasedPos(destCellPos);

		// 시작점 발견 (예약 진행)
		open[zeroBasedPos.Y, zeroBasedPos.X] = 10 * (Math.Abs(dest.Y - zeroBasedPos.Y) + Math.Abs(dest.X - zeroBasedPos.X));
		pq.Enqueue(new PQNode()
			{ priority = 10 * (Math.Abs(dest.Y - zeroBasedPos.Y) + Math.Abs(dest.X - zeroBasedPos.X)), y = zeroBasedPos.Y, x = zeroBasedPos.X });
		parent[zeroBasedPos.Y, zeroBasedPos.X] = new ZeroBasedPos(zeroBasedPos.Y, zeroBasedPos.X);

		while (pq.Count > 0) {
			// 제일 좋은 후보를 찾는다
			PQNode node = pq.Dequeue();
			// 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
			if (closed[node.y, node.x])
				continue;

			// 방문한다
			closed[node.y, node.x] = true;
			// 목적지 도착했으면 바로 종료
			if (node.y == dest.Y && node.x == dest.X)
				break;

			// 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
			for (int i = 0; i < _deltaY.Length; i++) {
				ZeroBasedPos next = new ZeroBasedPos(node.y + _deltaY[i], node.x + _deltaX[i]);

				// 유효 범위를 벗어났으면 스킵
				// 벽으로 막혀서 갈 수 없으면 스킵
				if (!ignoreDestCollision || next.Y != dest.Y || next.X != dest.X) {
					if (CanGo(ZeroBasedPos2Cell(next)) == false) // CellPos
						continue;
				}

				// 이미 방문한 곳이면 스킵
				if (closed[next.Y, next.X])
					continue;

				// 비용 계산 (휴리스틱)
				// g는 시작점에서 해당 지점까지 이동 비용, h는 목적지까지 가까운 정도. 둘 다 작을수록 좋다.
				int cost = 0; // node.G + _cost[i];
				int dist = 10 * ((dest.Y - next.Y) * (dest.Y - next.Y) + (dest.X - next.X) * (dest.X - next.X));
				
				// 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
				if (open[next.Y, next.X] < cost + dist)
					continue;

				// 예약 진행
				open[dest.Y, dest.X] = cost + dist;
				pq.Enqueue(new PQNode() { priority = cost + dist, y = next.Y, x = next.X });
				parent[next.Y, next.X] = new ZeroBasedPos(node.y, node.x);
			}
		}

		return CalcCellPathFromParent(parent, dest);
	}

	List<Vector2Int> CalcCellPathFromParent(ZeroBasedPos[,] parent, ZeroBasedPos dest) {
		List<Vector2Int> cells = new List<Vector2Int>();

		int y = dest.Y;
		int x = dest.X;
		while (parent[y, x].Y != y || parent[y, x].X != x) {
			cells.Add(ZeroBasedPos2Cell(new ZeroBasedPos(y, x)));
			ZeroBasedPos zeroBasedPos = parent[y, x];
			y = zeroBasedPos.Y;
			x = zeroBasedPos.X;
		}

		cells.Add(ZeroBasedPos2Cell(new ZeroBasedPos(y, x)));
		cells.Reverse();

		return cells;
	}

	ZeroBasedPos Cell2ZeroBasedPos(Vector2Int cellPos) => Cell2ZeroBasedPos(cellPos.x, cellPos.y);
	ZeroBasedPos Cell2ZeroBasedPos(int x, int y) {
		// CellPos -> ArrayPos
		return new ZeroBasedPos(MaxY - y, x - MinX);
	}

	Vector2Int ZeroBasedPos2Cell(ZeroBasedPos zeroBasedPos) {
		// ArrayPos -> CellPos
		return new Vector2Int(zeroBasedPos.X + MinX, MaxY - zeroBasedPos.Y);
	}

	#endregion
	
	/** 맵에 충돌 정보를 갱신하고, 서버상의 좌표를 이동시킨다 */
	public bool ApplyMove(Player player, Vector2Int destCellPos) {
		if (!CanGo(destCellPos, true)) return false;
		
		var curCellPos = player.Info.PosInfo;
		if (!IsValidCellPos(curCellPos.PosX, curCellPos.PosY)) return false;

		var curPos = Cell2ZeroBasedPos(curCellPos.PosX, curCellPos.PosY);
		if (players[curPos.Y, curPos.X] == player) {
			players[curPos.Y, curPos.X] = null;
		}

		var destPos = Cell2ZeroBasedPos(destCellPos);
		players[destPos.Y, destPos.X] = player;

		curCellPos.PosX = destCellPos.x;
		curCellPos.PosY = destCellPos.y;
		return true;
	}

	private bool IsValidCellPos(int x, int y) {
		if (x < MinX || x > MaxX) return false;
		if (y < MinY || y > MaxY) return false;
		return true;
	}

	public Player Find(Vector2Int cellPos) {
		if (!IsValidCellPos(cellPos.x, cellPos.y)) return null;

		var pos = Cell2ZeroBasedPos(cellPos);
		return players[pos.Y, pos.X];
	}

	public void ApplyLeave(GameObject gameObject) {
		// TODO
	}
}
