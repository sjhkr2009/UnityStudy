using System.Collections.Generic;
using System.Linq;

namespace Server.Game; 

public static class RoomManager {
    private static object _lock = new object();

    private static Dictionary<int, GameRoom> rooms = new Dictionary<int, GameRoom>();
    private static int nextRoomId = 1;

    public static GameRoom Create(int mapId) {
        GameRoom gameRoom = new GameRoom();
        gameRoom.Initialize(mapId);
        
        lock (_lock) {
            gameRoom.RoomId = nextRoomId;
            rooms.Add(nextRoomId, gameRoom);
            nextRoomId++;
        }

        return gameRoom;
    }

    public static bool Remove(int roomId) {
        lock (_lock) {
            return rooms.Remove(roomId);
        }
    }

    public static GameRoom Find(int roomId) {
        lock (_lock) {
            return rooms.TryGetValue(roomId, out var room) ? room : null;
        }
    }

    public static GameRoom First() {
        lock (_lock) {
            return rooms.Count > 0 ? rooms.First().Value : null;
        }
    }
}