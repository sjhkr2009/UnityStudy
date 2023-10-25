using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapManager {
	private GameObject map;
	public Grid CurrentGrid { get; private set; }

	public int MinX { get; internal set; }
	public int MaxX { get; internal set; }
	public int MinY { get; internal set; }
	public int MaxY { get; internal set; }
	public int SizeX => MaxX - MinX + 1;
	public int SizeY => MaxY - MinY + 1;

	private bool[,] collisionData;

	private static string GetMapName(int id) => $"Map_{id:000}";

	public void LoadMap(int mapId) {
		DestroyMap();

		string mapName = GetMapName(mapId);
		map = Director.Resource.Instantiate($"Map/{mapName}");
		map.name = "Map";
		CurrentGrid = map.GetOrAddComponent<Grid>();

		var collisionMap = CustomUtility.FindChild(map, "Tilemap_Collision");
		if (collisionMap != null)
			collisionMap.SetActive(false);

		TextAsset collisionText = Director.Resource.Load<TextAsset>($"Map/{mapName}");
		SetCollisionData(collisionText);
	}

	public void DestroyMap() {
		if (map == null) return;

		Object.Destroy(map);
		CurrentGrid = null;
	}

	private void SetCollisionData(TextAsset collitionInfoText) {
		StringReader reader = new StringReader(collitionInfoText.text);

		MinX = int.Parse(reader.ReadLine());
		MaxX = int.Parse(reader.ReadLine());
		MinY = int.Parse(reader.ReadLine());
		MaxY = int.Parse(reader.ReadLine()) + 1; // MaxY에서 1을 뺐으니 다시 더해준다. 어떤 이슈인지는 MapEditor.GenerateMapInfo 참고.

		int yCount = MaxY - MinY + 1;
		int xCount = MaxX - MinX + 1;
		collisionData = new bool[yCount, xCount];

		for (int y = 0; y < yCount; y++) {
			string line = reader.ReadLine();
			if (line == null) break;

			for (int x = 0; x < xCount; x++) {
				try {
					collisionData[y, x] = line[x] != '0';
				}
				catch (IndexOutOfRangeException e) {
					Debug.LogError(
						$"({x},{y}) 좌표를 찾을 수 없습니다 / Max:{MaxX},{MaxY} / Min: {MinX},{MinY} / Count: {xCount},{yCount} / Line: {line.Length} \n에러 메시지: {e}");
				}
			}
		}
	}

	public bool CanGo(Vector3Int cellPos) {
		// 맵 범위 밖이면 false
		if (cellPos.x < MinX || cellPos.x > MaxX || cellPos.y < MinY || cellPos.y > MaxY) return false;

		// 맵 배열 내에서의 인덱스를 구한다. 맵 왼쪽 위를 원점으로 간주한다. 
		int x = cellPos.x - MinX;
		int y = MaxY - cellPos.y;

		bool existCollision = collisionData[y, x];
		return !existCollision;
	}

	#region A* PathFinding

	// U D L R
	int[] _deltaY = new int[] { 1, -1, 0, 0 };
	int[] _deltaX = new int[] { 0, 0, -1, 1 };
	int[] _cost = new int[] { 10, 10, 10, 10 };

	public List<Vector3Int> FindPath(Vector3Int startCellPos, Vector3Int destCellPos, bool ignoreDestCollision = false) {
		// (y, x) 이미 방문했는지 여부 (방문 = closed 상태)
		bool[,] closed = new bool[SizeY, SizeX]; // CloseList

		// (y, x) 가는 길을 한 번이라도 발견했는지
		// 발견X => MaxValue
		// 발견O => Priority (= cost + dist)
		int[,] open = new int[SizeY, SizeX]; // OpenList
		for (int y = 0; y < SizeY; y++)
		for (int x = 0; x < SizeX; x++)
			open[y, x] = Int32.MaxValue;

		Pos[,] parent = new Pos[SizeY, SizeX];

		// 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
		PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

		// CellPos -> ArrayPos
		Pos pos = Cell2Pos(startCellPos);
		Pos dest = Cell2Pos(destCellPos);

		// 시작점 발견 (예약 진행)
		open[pos.Y, pos.X] = 10 * (Math.Abs(dest.Y - pos.Y) + Math.Abs(dest.X - pos.X));
		pq.Push(new PQNode()
			{ priority = 10 * (Math.Abs(dest.Y - pos.Y) + Math.Abs(dest.X - pos.X)), y = pos.Y, x = pos.X });
		parent[pos.Y, pos.X] = new Pos(pos.Y, pos.X);

		while (pq.Count > 0) {
			// 제일 좋은 후보를 찾는다
			PQNode node = pq.Pop();
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
				Pos next = new Pos(node.y + _deltaY[i], node.x + _deltaX[i]);

				// 유효 범위를 벗어났으면 스킵
				// 벽으로 막혀서 갈 수 없으면 스킵
				if (!ignoreDestCollision || next.Y != dest.Y || next.X != dest.X) {
					if (CanGo(Pos2Cell(next)) == false) // CellPos
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
				pq.Push(new PQNode() { priority = cost + dist, y = next.Y, x = next.X });
				parent[next.Y, next.X] = new Pos(node.y, node.x);
			}
		}

		return CalcCellPathFromParent(parent, dest);
	}

	List<Vector3Int> CalcCellPathFromParent(Pos[,] parent, Pos dest) {
		List<Vector3Int> cells = new List<Vector3Int>();

		int y = dest.Y;
		int x = dest.X;
		while (parent[y, x].Y != y || parent[y, x].X != x) {
			cells.Add(Pos2Cell(new Pos(y, x)));
			Pos pos = parent[y, x];
			y = pos.Y;
			x = pos.X;
		}

		cells.Add(Pos2Cell(new Pos(y, x)));
		cells.Reverse();

		return cells;
	}

	Pos Cell2Pos(Vector3Int cell) {
		// CellPos -> ArrayPos
		return new Pos(MaxY - cell.y, cell.x - MinX);
	}

	Vector3Int Pos2Cell(Pos pos) {
		// ArrayPos -> CellPos
		return new Vector3Int(pos.X + MinX, MaxY - pos.Y, 0);
	}

	#endregion
}
