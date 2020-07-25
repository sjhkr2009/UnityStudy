using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    /// <summary>
    /// Resource Manager에서 타입에 따른 이미지를 로드하기 위해 사용됩니다. 파라미터에 따라 Resource/Data 내에서 지정한 종류의 이미지를 가져옵니다.
    /// </summary>
    public enum ImageType
    {
        Base,
        Sub,
        Cocktail,
        Customer
    }

    /// <summary>
    /// Sound Manager에서 실행할 사운드의 타입을 결정합니다. Count는 사운드 타입의 개수를 나타내므로 항상 마지막에 위치해야 합니다.
    /// </summary>
    public enum SoundType
    {
        BGM,
        FX,
        LoopFX,
        Count
    }

    /// <summary>
    /// [사용되지 않음] 마우스 동작에 따른 UI 이벤트 실행을 위해 생성했으나, 대부분의 동작이 클릭(터치)으로 구성됨에 따라 EventHandler를 사용하지 않습니다.
    /// </summary>
    public enum EventType
    {
        Click
    }

    /// <summary>
    /// 각 캐릭터의 정보를 통해 매치되는 대사를 찾기 위해 사용됩니다. Resource 폴더의 Data/StoryTexts 에서 사용합니다.
    /// </summary>
    public enum CustomerType
    {
        None,
        Eagle = 1,
        Goni = 2

    }

    /// <summary>
    /// 칵테일의 태그를 정리한 자료입니다. 각 재료가 가지고 있으며 완성된 칵테일은 하위 재료의 태그를 모두 갖게 됩니다.
    /// </summary>
    public enum CocktailTag
    {
        Null,
        데킬라,
        보드카,
        위스키,
        진,
        럼,
        브랜디,
        주스,
        과일,
        시럽,
        오렌지,
        라임,
        레몬,
        석류,
        탄산,
        리큐어,
        아몬드,
        생강,
        주류,
        커피,
        민트,
        설탕,
        체리,
        음료수
    }

    /// <summary>
    /// 칵테일의 도수 (0 ~ CocktailMaxProof) 를 등급으로 반환하기 위한 열거형 자료입니다.
    /// </summary>
    public enum ProofGrade
    {
        매우약함 = 0,
        약함 = 1,
        중간 = 2,
        셈 = 3,
        매우셈 = 4
    }

    /// <summary>
    /// 칵테일의 최대 도수를 결정합니다. UI 출력에만 사용됩니다.
    /// </summary>
    public const float CocktailMaxProof = 40f;

    /// <summary>
    /// 칵테일 평가 시 내부적으로 0~100점으로 산출된 점수를, 어디를 기준으로 GOOD / SOSO / BAD로 분류할 지 결정합니다. 이 수치보다 높으면 GOOD 결과를 얻습니다.
    /// </summary>
    public const float Evaluate_GoodHigherThan = 70f;
    /// <summary>
    /// 칵테일 평가 시 내부적으로 0~100점으로 산출된 점수를, 어디를 기준으로 GOOD / SOSO / BAD로 분류할 지 결정합니다. 이 수치보다 낮으면 BAD 결과를 얻습니다.
    /// </summary>
    public const float Evaluate_BadLowerThan = 30f;

    /// <summary>
    /// 대화창에 텍스트 출력 시, 한 글자 당 몇 초에 걸쳐 출력할 지 결정합니다. 빠른 모드(DoTextSpeedFast)는 대화내용이 길 때 사용됩니다.
    /// </summary>
    public const float DoTextSpeed = 0.025f;
    /// <summary>
    /// 대화창에 텍스트 출력 시, 한 글자 당 몇 초에 걸쳐 출력할 지 결정합니다. 대화내용이 길 때 사용됩니다.
    /// </summary>
    public const float DoTextSpeedFast = 0.015f;

    /// <summary>
    /// 초당 칵테일 제조가 자동으로 진행되는 정도 (%) : 10이면 초당 10%씩 자동으로 진행됩니다.
    /// </summary>
    public const float CocktailMakingProcess = 10f;

    /// <summary>
    /// Select Base Material UI에서 표시되는 베이스 재료들의 크기 비율을 나타냅니다. 원본 이미지의 종횡비를 유지하면서 비율을 조정합니다.
    /// </summary>
    public const float ImageScale_BaseSelectUI = 0.8f;

    /// <summary>
    /// 베이스 재료 사진의 크기를 반환합니다.
    /// </summary>
    public static Vector2 baseMaterialSize => new Vector2(170f, 586f);
}
