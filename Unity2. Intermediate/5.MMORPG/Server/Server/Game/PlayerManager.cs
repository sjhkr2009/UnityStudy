using System.Collections.Generic;

namespace Server.Game;

public static class PlayerManager {
    private static object _lock = new object();

    private static Dictionary<int, Player> players = new Dictionary<int, Player>();
    private static int nextPlayerId = 1;

    public static Player Create() {
        Player player = new Player();
        
        lock (_lock) {
            player.Info.PlayerId = nextPlayerId;
            players.Add(nextPlayerId, player);
            nextPlayerId++;
        }

        return player;
    }

    public static bool Remove(int playerId) {
        lock (_lock) {
            return players.Remove(playerId);
        }
    }

    public static Player Find(int playerId) {
        lock (_lock) {
            return players.TryGetValue(playerId, out var player) ? player : null;
        }
    }
}