using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 실제로 사용하는 부분은 L02_DataManager 에서 구현하기로 한다.
public class L01_DataLoad
{
    // 수치와 관련된 부분은 하드코딩하지 않고 따로 빼 두는 경우가 많다.
    // 그렇지 않으면 출시된 게임의 밸런스 조정 및 업데이트 시 하드코딩된 부분을 직접 수정 후 다시 빌드하여 배포해야 하기 때문이다.

    // 데이터는 항상 들고 있어야 하니 Clear는 불필요하다.

    // JSON 문법
    // [] -> 배열(List)을 나타냄
    // {} -> 구조체(Struct)를 나타냄


    public Dictionary<int, L01_Stat> StatDict { get; private set; } = new Dictionary<int, L01_Stat>();

    public void Init()
    {
        // 우선 파일 포맷을 맞춰준다. 파일 형식은 JSON이나 XML이 주로 사용된다. (최근엔 JSON, 예전엔 XML. 원본을 디자이너가 엑셀 파일로 작성하고 서버나 클라에서는 JSON으로 바꿔 사용하는 식이다.)

        TextAsset textAsset = A01_Manager.Resource.Load<TextAsset>("Data/StatData");
        Debug.Log($"불러온 데이터 : {textAsset.text}");

        // FromJson으로 데이터를 불러온다.
        // JSON 내에 정의된 변수명에 따라, stats 리스트에 level, hp, attack 변수가 지정된 값으로 채워질 것이다.
        L01_StatData data = JsonUtility.FromJson<L01_StatData>(textAsset.text);

        // 데이터는 ID값 등 식별가능한 고유 정보를 통해 불러오므로, 딕셔너리 형태로 만들어준다. 여기서는 레벨을 고유 값으로 사용한다.
        foreach (L01_Stat stat in data.stats)
            StatDict.Add(stat.level, stat);


    }
}

// 텍스트 데이터는 Serializable을 통해 메모리에 읽어와 저장해둔다.
// stats, level, hp 등의 변수명은 JSON 파일에서 선언한 것과 동일해야 한다.
[Serializable]
public class L01_StatData
{
    public List<L01_Stat> stats = new List<L01_Stat>();
}

[Serializable]
public class L01_Stat
{
    // 변수는 public으로 선언하거나 [SerializeField]를 붙여야 한다.
    public int level;
    public int hp;
    public int attack;
}