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
        태그1,
        태그2,
        태그3
    }

    public const float CocktailMaxProof = 50f;
}
