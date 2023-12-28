using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Server.Data; 

// 해당 클래스는 저장하고 있는 자료들을 Value로, 자료를 식별할 수 있는 요소를 Key로 설정하여 DIctionary를 반환해야 합니다.
public interface ILoader<Key, Value> {
    Dictionary<Key, Value> LoadData();
}

public static class DataManager {
    public static Dictionary<int, Stat> StatDict { get; private set; }
    public static Dictionary<int, Skill> SkillDict { get; private set; }

    public static void Init() {
        StatDict = LoadJson<StatData, int, Stat>("StatData").LoadData();
        SkillDict = LoadJson<SkillData, int, Skill>("SkillData").LoadData();
    }
    
    static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value> {
        var dataPath = Path.Combine(ConfigManager.Config.dataPath, path);

        var text = File.Exists(dataPath) ? File.ReadAllText(dataPath) : string.Empty;
        return JsonConvert.DeserializeObject<Loader>(text);
    }
}
