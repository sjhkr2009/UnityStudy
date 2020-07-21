using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ImageType
    {
        Base,
        Sub,
        Cocktail,
        Customer
    }

    public enum SoundType
    {
        BGM,
        FX,
        Count
    }

    public enum EventType
    {
        Click
    }

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

    public const float CocktailMaxProof = 50f;

    public const float Evaluate_GoodHigherThan = 70f;
    public const float Evaluate_BadLowerThan = 30f;

    public static Vector2 baseMaterialSize => new Vector2(170f, 586f);
}
