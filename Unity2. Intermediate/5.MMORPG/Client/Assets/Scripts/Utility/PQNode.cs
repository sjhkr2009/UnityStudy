using System;

public struct PQNode : IComparable<PQNode> {
    /** 낮을수록 우선순위 높음 */
    public int priority;
    //public int cost;
    public int y;
    public int x;

    public int CompareTo(PQNode other)
    {
        if (priority == other.priority)
            return 0;
        return priority < other.priority ? 1 : -1;
    }
}
