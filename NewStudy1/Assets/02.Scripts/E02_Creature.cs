abstract class Creature
{
    protected Creature(CreatureType type)
    {
        creatureType = type;
    }

    protected enum CreatureType
    {
        None,
        Player = 1,
        Monster = 2
    }
    protected CreatureType creatureType = CreatureType.None;

    public string name;
    private int _hp;
    private int _power;
    public int maxHp { get; protected set; }
    public float damageDifference { get; protected set; }

    public void HealTo(int value)
    {
        _hp = value;
    }
    public void SetInfo(int hp, int power)
    {
        _hp = hp;
        _power = power;
    }
    public int GetHp() { return _hp; }
    public int GetPower() { return _power; }
    public bool IsDead() { return _hp <= 0; }
    public void OnDamaged(int damage)
    {
        _hp -= damage;
        if (_hp < 0) _hp = 0;
    }
    public void OnDamaged(Creature attacker, int damage)
    {
        _hp -= damage;
        if (_hp < 0) _hp = 0;
    }
    public void OnDamaged(Creature attacker)
    {
        _hp -= attacker.GetPower();
        if (_hp < 0) _hp = 0;
    }
    public void Attack(Creature attackedObject)
    {
        attackedObject.OnDamaged(GetHp());
    }
}
