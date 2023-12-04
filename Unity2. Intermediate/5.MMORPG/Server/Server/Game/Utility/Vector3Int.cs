namespace Server.Game.Utility; 

public struct Vector2Int {
    public int x;
    public int y;

    public Vector2Int(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Vector2Int up => new Vector2Int(0, 1);
    public static Vector2Int down => new Vector2Int(0, -1);
    public static Vector2Int left => new Vector2Int(-1, 0);
    public static Vector2Int right => new Vector2Int(1, 0);
    public static Vector2Int zero => new Vector2Int(0, 0);
    public static Vector2Int one => new Vector2Int(1, 1);

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
}