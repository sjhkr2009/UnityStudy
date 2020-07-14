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

class Tequilla : BaseMaterials
{
    public Tequilla() : base(1)
    {
        Name = "데킬라";
        Info = "용설란 수액으로 만든 멕시코산 술";
    }
}

class Vodka : BaseMaterials
{
    public Vodka() : base(2)
    {
        Name = "보드카";
        Info = "러시아의 대표적인 고농도 증류주";
    }
}

class Whisky : BaseMaterials
{
    public Whisky() : base(3)
    {
        Name = "위스키";
        Info = "맥아를 주원료로 하며 영미권에서 유행하는 술.";
    }
}

class Gin : BaseMaterials
{
    public Gin() : base(3)
    {
        Name = "진";
        Info = "쥬니퍼베리로 향을 낸 증류주. 네덜란드에서 사랑받는 술이다.";
    }
}

class Rum : BaseMaterials
{
    public Rum() : base(3)
    {
        Name = "럼";
        Info = "감미로운 향을 자랑하는 술. 선원들 사이에서 인기가 많았었기 때문에 '뱃사람의 술'로도 불린다.";
    }
}

class Brandy : BaseMaterials
{
    public Brandy() : base(3)
    {
        Name = "브랜디";
        Info = "???";
    }
}