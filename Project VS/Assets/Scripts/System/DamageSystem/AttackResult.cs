public struct AttackResult {
    public bool isHit;
    public bool isDead;

    public AttackResult(bool isHit, bool isDead) {
        this.isHit = isHit;
        this.isDead = isDead;
    }

    public static AttackResult None => new AttackResult(false, false);
    public static AttackResult Hit => new AttackResult(true, false);
    public static AttackResult Dead => new AttackResult(true, true);
}
