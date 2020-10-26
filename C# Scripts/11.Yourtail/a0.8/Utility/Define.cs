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
    /// Warning UI 팝업창에서 매개변수로 받아서, 현재 표시해야 할 경고의 종류에 따라 UI의 안내 텍스트와 버튼의 동작을 세팅합니다.
    /// </summary>
    public enum WarningType
    {
        QuitApp
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
        Swan = 4,
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
        음료수,
        캄파리,
        베르무트,
        콜라
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

    #region 최대치 및 개수 관련

    /// <summary>
    /// 베이스 재료(스피릿)의 총 개수를 나타냅니다.
    /// </summary>
    public const int SpiritCount = 6;
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
    /// 새들의 최대 레벨입니다. 해당 새의 ID를 인덱스로 사용하며, 인덱스 0에는 최소값인 1이 할당되어 있습니다.
    /// </summary>
    public static int[] BirdMaxLevel { get; private set; } = { 1, 6, 6, 4, 5, 5 };
    /// <summary>
    /// 칵테일의 최대 도수를 결정합니다. UI 출력에만 사용됩니다.
    /// </summary>
    public const float CocktailMaxProof = 40f;

    #endregion

    #region 속도 제어 관련

    /// <summary>
    /// 대화창에 텍스트 출력 시, 한 글자 당 몇 초에 걸쳐 출력할 지 결정합니다. 빠른 모드(DoTextSpeedFast)는 대화내용이 길 때 사용됩니다.
    /// </summary>
    public const float DoTextSpeed = 0.025f;
    /// <summary>
    /// 대화창에 텍스트 출력 시, 한 글자 당 몇 초에 걸쳐 출력할 지 결정합니다. 대화내용이 길 때 사용됩니다.
    /// </summary>
    public const float DoTextSpeedFast = 0.015f;
    /// <summary>
    /// 초당 칵테일 제조가 자동으로 진행되는 정도 (%) : 20이면 초당 20%씩 자동으로 진행됩니다.
    /// </summary>
    public const float CocktailMakingProcess_Default = 10f;
    public const float CocktailMakingProcess_Max = 75f;

    #endregion

    #region 크기 및 해상도 관련

    /// <summary>
    /// UI 캔버스들의 레퍼런스 해상도를 나타냅니다.
    /// </summary>
    public static Vector2 UIRefResolution => new Vector2(1920, 1080);
    /// <summary>
    /// [사용되지 않음] Select Base Material UI에서 표시되는 베이스 재료들의 크기 비율을 나타냅니다. 원본 이미지의 종횡비를 유지하면서 비율을 조정합니다.
    /// </summary>
    public const float ImageScale_BaseSelectUI = 1f;
    /// <summary>
    /// 게임 좌측 하단 메뉴 아이콘의 간격을 나타냅니다.
    /// </summary>
    public const float MenuIconSpacing = 120f;
    /// <summary>
    /// Bird Info Window에서 스토리를 열람하는 버튼이 나열된 간격을 나타냅니다.
    /// </summary>
    public const int StoryButtonsSpacing = 125;
    /// <summary>
    /// 재료 사진의 크기를 반환합니다.
    /// </summary>
    public static Vector2 MaterialSize => new Vector2(170f, 586f);
    /// <summary>
    /// 새 컬렉션의 슬라이더 길이입니다. 하트 아이콘을 슬라이더 위에 일정 간격으로 배치하기 위해 사용됩니다.
    /// </summary>
    public const int BirdLevelSliderWidth = 1000;
    /// <summary>
    /// 가로로 스크롤되는 여러 개의 창이 있을 때, 전체 창의 개수와 현재 창이 몇 번째인지가 점으로 표기됩니다. 강조된 점의 크기를 얼마나 키울지 결정합니다.
    /// 값이 1.5일 경우, 첫 번째 페이지를 보고 있을 때 첫 번째 점이 다른 점보다 1.5배 커집니다. (현재 SelectMaterialUI 부재로 선택창에서 사용)
    /// </summary>
    public const float SelectedDotIconScale = 1.4f;

    #endregion

    #region 데이터 변환 및 기준치 관련

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
    /// 호감도 단계별로 필요한 호감도의 양입니다. 호감도 단계가 3 → 4단계로 상승하기 위해서는 RequiredEXP[3] 만큼의 호감도가 필요합니다.
    /// </summary>
    public static int[] RequiredEXP => new int[MaxLevel] { 1, 10, 15, 20, 25 };

    /// <summary>
    /// 게임 좌측 하단의 메뉴 아이콘 클릭 시, 메뉴 아이콘들이 나오는 시간을 설정합니다.
    /// </summary>
    public const float OpenMenuDuration = 0.25f;

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

    #endregion


}
