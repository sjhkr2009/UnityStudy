using System.Collections.Generic;
using Google.Protobuf.Protocol;

namespace Server.Game;

public static class ObjectManager {
    private static object _lock = new object();

    private static Dictionary<int, Player> players = new Dictionary<int, Player>();
    
    private static int counter = 1;
    
    // 첫 1비트는 사용하지 않음, 다음 7비트는 타입, 나머지 24비트로 id를 나타내기로 한다.
    private static int GenerateId(GameObjectType type) {
        lock (_lock) {
            return ((int)type << 24) | (counter++);
        }
    }

    public static GameObjectType GetObjectTypeBy(int id) {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public static GameObjectType GetObjectType(this GameObject gameObject) => GetObjectTypeBy(gameObject.Id);
    

    public static T Create<T>() where T : GameObject, new() {
        T gameObject = new T();

        lock (_lock) {
            gameObject.Id = GenerateId(gameObject.ObjectType);

            if (gameObject.ObjectType == GameObjectType.Player) {
                players.Add(gameObject.Id, gameObject as Player);
            }
        }

        return gameObject;
    }

    public static bool Remove(int playerId) {
        var type = GetObjectTypeBy(playerId);
        
        lock (_lock) {
            if (type == GameObjectType.Player) return players.Remove(playerId);
        }

        return false;
    }

    public static Player Find(int playerId) {
        var type = GetObjectTypeBy(playerId);
        
        lock (_lock) {
            if (type == GameObjectType.Player && players.TryGetValue(playerId, out var player)) {
                return player;
            }
        }

        return null;
    }
}