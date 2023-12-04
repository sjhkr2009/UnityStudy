namespace Server.Game.Utility;

/** (0,0)을 최소값으로 사용하는 좌표값. */
public struct ZeroBasedPos {
    public ZeroBasedPos(int y, int x) { Y = y; X = x; }
    public int Y;
    public int X;
}