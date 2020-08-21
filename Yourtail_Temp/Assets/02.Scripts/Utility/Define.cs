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
        Parrot = 2,
        Flamingo = 3,
        Goni = 4,
        Penguin = 5

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

    public enum Reaction
    {
        BAD = -1,
        SOSO = 0,
        GOOD = 1
    }

    /// <summary>
    /// 베이스 재료의 최대 개수를 지정합니다.
    /// </summary>
    public const int MaxBaseMaterial = 1;
    /// <summary>
    /// 부재료의 최대 개수를 지정합니다.
    /// </summary>
    public const int MaxSubMaterial = 3;
    /// <summary>
    /// 캐릭터가 가질 수 있는 최대 레벨을 지정합니다.
    /// </summary>
    public const int MaxLevel = 5;


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
    /// 만들어진 칵테일에 대한 점수(0~100)를 GOOD/SOSO/BAD의 세 가지 결과로 분류하여, 1 / 0 / -1 의 정수로 반환합니다.
    /// </summary>
    /// <param name="score">DataManager 에서 계산되어 있는 칵테일 점수 입력</param>
    /// <returns></returns>
    public static int CocktailScoreToGrage(float score)
    {
        if (score > Evaluate_GoodHigherThan) return 1;
        if (score < Evaluate_BadLowerThan) return -1;
        return 0;
    }

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
    public const float ImageScale_BaseSelectUI = 1f;

    /// <summary>
    /// 재료 사진의 크기를 반환합니다.
    /// </summary>
    public static Vector2 MaterialSize => new Vector2(170f, 586f);

    /// <summary>
    /// 호감도 단계별로 필요한 호감도의 양입니다. 호감도 단계가 3 → 4단계로 상승하기 위해서는 RequiredEXP[3] 만큼의 호감도가 필요합니다.
    /// </summary>
    public static int[] RequiredEXP => new int[MaxLevel] { 1, 10, 15, 20, 25 };

    /// <summary>
    /// 게임 좌측 하단의 메뉴 아이콘 클릭 시, 메뉴 아이콘들이 나오는 시간을 설정합니다.
    /// </summary>
    public const float OpenMenuDuration = 0.25f;
    /// <summary>
    /// 게임 좌측 하단 메뉴 아이콘의 간격을 나타냅니다.
    /// </summary>
    public const float MenuIconSpacing = 120f;

    /// <summary>
    /// Bird Info Window에서 스토리를 열람하는 버튼이 나열된 간격을 나타냅니다.
    /// </summary>
    public const int StoryButtonsSpacing = 125;

    /// <summary>
    /// 칵테일의 도수를 시각적으로 표시할 때, 도수에 맞는 색상으로 변환시킵니다. (낮을수록 청록색, 높을수록 빨강색 계열을 갖게 됩니다)
    /// </summary>
    /// <param name="proof">도수를 입력하세요. 퍼센트를 그대로 입력해도 되지만, 최대 도수와 비교하여 0과 1 사이의 숫자로 입력하는 것이 권장됩니다.</param>
    /// <returns></returns>
    public static Color ProofToColor(float proof)
    {
        if (proof > 1f) proof = proof / CocktailMaxProof;
        
        float hue = Mathf.Clamp(0.5f - (proof * 0.6f), 0f, 0.5f);
        return Color.HSVToRGB(hue, 0.5f, 1f);
    }

    /// <summary>
    /// 도수를 등급으로 변환할 때 기준점을 나타냅니다. 인덱스 n의 도수 이하인 칵테일은 n등급을 갖게 됩니다. 예를 들어, 5도 칵테일의 도수 등급은 0 입니다.
    /// </summary>
    public static int[] ProofGradeCriterion { get; private set; } = { 5, 10, 19, 30, 40 };

    /// <summary>
    /// 새 컬렉션의 슬라이더 길이입니다. 하트 아이콘을 슬라이더 위에 일정 간격으로 배치하기 위해 사용됩니다.
    /// </summary>
    public const int BirdLevelSliderWidth = 1000;
}
