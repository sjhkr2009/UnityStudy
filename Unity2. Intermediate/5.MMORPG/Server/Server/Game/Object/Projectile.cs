using Google.Protobuf.Protocol;
using Server.Data;

namespace Server.Game; 

public class Projectile : GameObject {
    public Projectile() : base(GameObjectType.Projectile) { }
    
    public Skill Data { get; set; }

    public virtual void Update() { }
}