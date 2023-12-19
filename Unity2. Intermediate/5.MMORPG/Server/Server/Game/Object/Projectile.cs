using Google.Protobuf.Protocol;

namespace Server.Game; 

public class Projectile : GameObject {
    public Projectile() : base(GameObjectType.Projectile) { }

    public virtual void Update() { }
}