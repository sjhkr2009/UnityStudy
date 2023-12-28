using System;
using System.Collections.Generic;
using Google.Protobuf.Protocol;

namespace Server.Data;

[Serializable]
public class StatData : ILoader<int, Stat> {
    public List<Stat> stats = new List<Stat>();

    public Dictionary<int, Stat> LoadData() {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

        foreach (Stat stat in stats)
            dict.Add(stat.level, stat);

        return dict;
    }
}

[Serializable]
public class SkillData : ILoader<int, Skill> {
    public List<Skill> skills = new List<Skill>();

    public Dictionary<int, Skill> LoadData() {
        Dictionary<int, Skill> dict = new Dictionary<int, Skill>();

        foreach (var stat in skills)
            dict.Add(stat.id, stat);

        return dict;
    }
}

[Serializable]
public class Stat {
    public int level;
    public int hp;
    public int attack;
    public int totalExp;
}

[Serializable]
public class Skill {
    public int id;
    public string name;
    public float cooldown;
    public int damage;
    public SkillType skillType;
    public ProjectileInfo Projectile;
}

[Serializable]
public class ProjectileInfo {
    public string name;
    public int range;
    public float speed;
    public string prefab;
}
