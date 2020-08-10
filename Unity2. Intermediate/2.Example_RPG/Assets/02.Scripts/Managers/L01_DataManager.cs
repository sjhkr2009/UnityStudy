using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L01_DataManager
{
    // 수치와 관련된 부분은 하드코딩하지 않고 따로 빼 두는 경우가 많다.
    // 그렇지 않으면 출시된 게임의 밸런스 조정 및 업데이트 시 하드코딩된 부분을 직접 수정 후 다시 빌드하여 배포해야 하기 때문이다.

    // 데이터는 항상 들고 있어야 하니 Clear는 불필요하다.

    // JSON 문법
    // [] -> 배열(List)을 나타냄
    // {} -> 구조체(Struct)를 나타냄

    public void Init()
    {
        // 우선 파일 포맷을 맞춰준다. 파일 형식은 JSON이나 XML이 주로 사용된다. (최근엔 JSON, 예전엔 XML. 원본을 디자이너가 엑셀 파일로 작성하고 서버나 클라에서는 JSON으로 바꿔 사용하는 식이다.)

        TextAsset textAsset = A01_Manager.Resource.Load<TextAsset>("Data/StatData");
        Debug.Log($"불러온 데이터 : {textAsset.text}");

        // FromJson으로 데이터를 불러온다.
        // JSON 내에 정의된 변수명에 따라, stats 리스트에 level, hp, attack 변수가 지정된 값으로 채워질 것이다.
        StatData data = JsonUtility.FromJson<StatData>(textAsset.text);


    }
}

// 텍스트 데이터는 Serializable을 통해 메모리에 읽어와 저장해둔다.
// stats, level, hp 등의 변수명은 JSON 파일에서 선언한 것과 동일해야 한다.
[Serializable]
public class StatData
{
    public List<Stat> stats = new List<Stat>();
}

[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;
}