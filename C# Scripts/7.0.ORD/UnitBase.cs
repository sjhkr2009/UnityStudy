using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum UnitLevel { Common, Uncommon, Special, Rare, Legendary, Hidden, Changed, Transcendent, Immortal, Eternal, Limited, SelectionWisp = -1 }

public abstract class UnitBase : MonoBehaviour
{
    [BoxGroup("UnitInfo")] public int commonNumber;
    [BoxGroup("UnitInfo")] public string unitName;
    [BoxGroup("UnitInfo")] public List<UnitBase> materials = new List<UnitBase>();
    [BoxGroup("UnitInfo")] public UnitLevel unitLevel;
    [BoxGroup("UnitInfo")] public int printPriority; //높을수록 먼저 표기

    [ReadOnly] public int currentUnitNumber;
    [ReadOnly] public int testUnitNumber;
    [SerializeField, ReadOnly] List<UnitBase>  currentMaterialList = new List<UnitBase>();
    [SerializeField, ReadOnly] List<UnitBase> requiredCommonList = new List<UnitBase>();
    [ReadOnly] public bool canMake = false;

    protected virtual void Awake()
    {
        commonNumber = CommonUnitCount();
    }

    protected virtual void Start()
    {
        CheckCommonNumber();
        printPriority += (int)unitLevel * 100;
    }

    /// <summary>
    /// 게임 시작 시 흔함개수를 체크합니다.
    /// </summary>
    public void CheckCommonNumber()
    {
        int count = 0;
        foreach (var material in materials) count += material.commonNumber;
        if (count != commonNumber && unitName != "레일리(희귀함)" && unitName != "선택위습") Debug.Log($"흔함개수 확인 바람: {gameObject.name}의 흔함개수 = {commonNumber}");
    }

    /// <summary>
    /// 실행 최초에 흔함개수가 비어 있을 경우, 하위 유닛을 탐색하여 채웁니다.
    /// </summary>
    /// <returns></returns>
    public int CommonUnitCount()
    {
        if (commonNumber > 0) return commonNumber;
        else
        {
            foreach (var material in materials) commonNumber += material.CommonUnitCount();
            return commonNumber;
        }
    }


    /// <summary>
    /// 현재 존재하는 재료들을 최하위까지 순서대로 탐색하여, 재료 리스트 반환하고 재료 충족 여부를 체크합니다.
    /// </summary>
    /// <returns></returns>
    public List<UnitBase> FindCurrentMaterial()
    {
        List<UnitBase> materialList = new List<UnitBase>();
        canMake = true;

        //흔함의 재료인 선택위습이 없어서 선택위습에서 이 함수가 실행된 경우, 빈 리스트로 반환하기
        if (materials == null || materials.Count == 0)
        {
            return materialList;
        }

        //각 하위 재료의 현재 개수를 임시 변수에 기록해두고 -> 매 재료마다 개수를 초기화하면 안 되니 다른 함수에서 실행
        //foreach (var material in materials) material.testUnitNumber = material.currentUnitNumber; //이 코드는 취소

        //각 하위 재료를 탐색하여
        foreach (var material in materials)
        {
            if (material.testUnitNumber > 0) //재료가 남아있을 경우
            {
                material.testUnitNumber--; //임시로 재료를 1개 차감하고 (중복 재료 체크를 위해 차감하되, 체크 용도이므로 실제로 줄이면 안 된다)
                materialList.Add(material); //재료 리스트에 추가한다
            }
            else //재료가 남아있지 않을 경우
            {
                if(material.unitLevel == UnitLevel.Common) UnitManager.instance.requiredCommonList.Add(material); //그 없는 재료가 흔함이면 필요 흔함 리스트에 넣는다. 이 리스트는 지금 부족한 흔함개수만 체크하며, 이 함수가 다시 실행되면 초기화된다.
                foreach (var underMaterial in material.FindCurrentMaterial()) //그 재료에 대한 하위 재료를 탐색하여
                {
                    materialList.Add(underMaterial); //해당 리스트의 남아있는 재료들을 이 유닛의 재료 리스트에 추가한다.
                }
            }
        }
        if (CommonCount(materialList) != commonNumber) canMake = false; //재료 리스트의 흔함개수가 유닛의 흔함개수와 동일하지 않다면, 재료가 부족하므로 만들 수 없다.
        return materialList; //현재 존재하는 재료들의 리스트를 반환한다.
    }

    /// <summary>
    /// 리스트 내 유닛들의 흔함개수 합계를 반환합니다.
    /// </summary>
    /// <param name="units">흔함개수를 확인할 UnitBase 리스트</param>
    /// <returns></returns>
    int CommonCount(List<UnitBase> units)
    {
        int count = 0;
        foreach (var unit in units) count += unit.commonNumber;
        return count;
    }

    /// <summary>
    /// 이 유닛을 조합합니다. 하위 재료가 충족되어 있어야 합니다.
    /// </summary>
    [Button]
    public void Combine()
    {
        UnitManager.instance.UnitCountReset(); //개수 체크용 숫자를 실제 유닛 보유숫자와 같게 초기화해준다.
        currentMaterialList = FindCurrentMaterial(); //재료 충족 여부를 체크한다.
        if (canMake) //만약 이 유닛을 만들 수 있으면
        {
            foreach (var material in currentMaterialList) //각 재료를 차감하고
            {
                material.currentUnitNumber--;
            }
            currentUnitNumber++; //이 유닛의 개수를 1개 증가시킨다
        }
        else //만들 수 없으면
        {
            Debug.Log("재료 부족");
            string printText = ListToString(UnitManager.instance.requiredCommonList); //부족한 흔함 개수를 텍스트로 만들어 출력
            Debug.Log($"부족한 흔함개수: " + printText);
        }
    }

    /// <summary>
    /// 이 유닛의 개수를 1개 추가합니다.
    /// </summary>
    [Button]
    public void AddUnit()
    {
        currentUnitNumber++;
    }

    /// <summary>
    /// UnitBase 리스트 내의 요소를 "A, B, C, ..." 형태로 나열된 string으로 반환합니다.
    /// </summary>
    /// <param name="list">변환할 유닛 리스트</param>
    /// <returns></returns>
    string ListToString(List<UnitBase> list)
    {
        string printText = "";
        List<string> submaterials = new List<string>();
        foreach (var item in list)
        {
            submaterials.Add(item.unitName); //리스트 내 각 유닛의 이름을 string 형태로 새로운 리스트에 나열
        }
        submaterials = MaterialOverlapReplace(submaterials); //나열된 이름 리스트에서 중복된 요소를 n개 형태로 표기
        for (int i = 0; i < submaterials.Count; i++)
        {
            printText += submaterials[i]; 
            if (i != submaterials.Count - 1) printText += ", "; //리스트 사이사이에 반점을 넣어 콘솔에 출력할 메시지를 만든다.
        }
        return printText;
    }

    /// <summary>
    /// 이 유닛의 조합식을 출력합니다.
    /// </summary>
    [Button("조합식 보기")]
    public void PrintSubmaterials()
    {
        string printText = ListToString(materials);
        Debug.Log($"{unitName}의 재료: " + printText);
    }

    /// <summary>
    /// 리스트 내 중복된 요소를 검출하여, 중복된 재료 'A','A','A'를 'A 3개'의 형태로 변환합니다.
    /// </summary>
    /// <param name="checkList"></param>
    /// <returns></returns>
    List<string> MaterialOverlapReplace(List<string> checkList)
    {
        Dictionary<string, int> overlapMaterials = new Dictionary<string, int>();
        List<string> newTextList= new List<string>();

        foreach (var item in checkList)
        {
            if (overlapMaterials.ContainsKey(item)) overlapMaterials[item]++;
            else overlapMaterials.Add(item, 1);
        }
        foreach (var newItem in overlapMaterials)
        {
            newTextList.Add($"{newItem.Key} {newItem.Value}개");
        }

        return newTextList;
    }

    /// <summary>
    /// 유닛 조합에 필요한 흔함개수를 출력합니다.
    /// </summary>
    [Button("흔함개수 출력")]
    public void PrintCommonNumber()
    {
        string printText = "";
        List<string> printTextList = new List<string>();
        foreach (var unit in SortUnitList(MaterialCommonNumber(this))) printTextList.Add(unit.unitName);

        printTextList = MaterialOverlapReplace(printTextList);

        for (int i = 0; i < printTextList.Count; i++)
        {
            if (i == printTextList.Count - 1) printText += printTextList[i];
            else printText += printTextList[i] + ", ";
        }
        Debug.Log($"{unitName}의 흔함개수: " + printText);
    }

    /// <summary>
    /// 유닛 조합을 위해 현재 부족한 흔함개수를 출력합니다. 조합이 가능할 경우 조합 가능하다고 출력합니다.
    /// </summary>
    [Button("부족한 흔함 출력")]
    public void PrintRequiredCommon()
    {
        UnitManager.instance.UnitCountReset();
        //UnitManager.instance.requiredCommonList.Clear();
        currentMaterialList = FindCurrentMaterial();
        if (UnitManager.instance.requiredCommonList.Count == 0)
        {
            Debug.Log("조합 가능");
            return;
        }
        string printText = ListToString(UnitManager.instance.requiredCommonList);
        Debug.Log($"부족한 흔함개수: " + printText);
    }

    /// <summary>
    /// 유닛의 하위 재료로 쓰이는 흔함들을 리스트로 반환합니다. 각 하위 재료를 탐색해 흔함이면 리스트에 넣고, 상위 유닛이면 흔함이 나올 때까지 다시 하위 재료를 탐색합니다.
    /// </summary>
    /// <param name="unit">흔함재료를 탐색할 상위 유닛. 하위재료가 없거나 흔함인 경우 동작하지 않습니다.</param>
    /// <returns></returns>
    public List<UnitBase> MaterialCommonNumber(UnitBase unit)
    {
        List<UnitBase> commonList = new List<UnitBase>();

        if(unit.unitLevel == UnitLevel.Common)
        {
            commonList.Add(unit);
            return commonList;
        }
        if (unit.materials.Count == 0) return commonList;

        foreach (var material in materials)
        {
            if(material.unitLevel == UnitLevel.Common)
            {
                commonList.Add(material);
            }

            else
            {
                foreach (var item in material.MaterialCommonNumber(material)) commonList.Add(item);
            }
        }
        return commonList;
    }
    
    /// <summary>
    /// 각 유닛들이 들어있는 리스트를 넣으면, 우선순위에 따라 정렬된 리스트를 반환합니다.
    /// </summary>
    /// <param name="unitList">정렬이 필요한 재료 리스트</param>
    /// <returns></returns>
    public List<UnitBase> SortUnitList(List<UnitBase> unitList)
    {
        for (int j = 0; j < unitList.Count; j++)
        {
            for (int i = 0; i < unitList.Count - 1; i++)
            {
                if(unitList[i].printPriority < unitList[i + 1].printPriority)
                {
                    UnitBase temp = unitList[i + 1];
                    unitList[i + 1] = unitList[i];
                    unitList[i] = temp;
                }
            }
        }
        return unitList;
    }

    /// <summary>
    /// 유닛 리스트를 넣으면 해당 유닛의 이름을 담은 string 리스트로 반환합니다.
    /// </summary>
    /// <param name="unitList">string으로 변환할 유닛 리스트</param>
    /// <returns></returns>
    public List<string> UnitbaseToString(List<UnitBase> unitList)
    {
        List<string> textList = new List<string>();
        foreach (var unit in unitList) textList.Add(unit.unitName);
        return textList;
    }
}
