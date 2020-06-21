using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMaterials : CocktailMaterials
{
    public BaseMaterials() : base(MaterialType.Base)
    {
        SetID(0);
    }
    public BaseMaterials(int id) : base(MaterialType.Base)
    {
        SetID(id);
        image = GameManager.Resource.LoadImage(Define.ImageType.Base, id);
    }
}

class Rum : BaseMaterials
{
    public Rum() : base(1)
    {
        Name = "럼";
    }
}

class Brandy : BaseMaterials
{
    public Brandy() : base(2)
    {
        Name = "브랜디";
    }
}