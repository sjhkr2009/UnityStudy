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

class Bmt_Tequilla : BaseMaterials
{
    public Bmt_Tequilla() : base(1)
    {
        Name = "데킬라";
        Info = "용설란의 수액으로 만든 날카로운 풍미의 멕시코 술. 할리스코 주에서 주로 만들어지며, 이름은 해당 주의 지역명 '데킬라'에서 유래하였다.";
        Tags.Add(Define.CocktailTag.데킬라);
    }
}

class Bmt_Vodka : BaseMaterials
{
    public Bmt_Vodka() : base(2)
    {
        Name = "보드카";
        Info = "러시아에서 사랑받는 증류주. 무색 무취에 중성적인 맛을 가지고 있으며, 도수가 높아 소독약으로 쓰이기도 했다. 물론 주스로 희석시켜 도수를 낮출 수 있다.";
        Tags.Add(Define.CocktailTag.보드카);
    }
}

class Bmt_Whisky : BaseMaterials
{
    public Bmt_Whisky() : base(3)
    {
        Name = "위스키";
        Info = "곡류를 원료로 발효와 증류를 거친 호박색의 술. 술이 지닌 본연의 맛을 중시한다.";
        Tags.Add(Define.CocktailTag.위스키);
    }
}

class Bmt_Gin : BaseMaterials
{
    public Bmt_Gin() : base(4)
    {
        Name = "진";
        Info = "곡물 원료의 증류주에서 주니퍼베리로 향을 낸 술. 네덜란드에서 탄생했으며, 미국에 금주법이 시행될 당시 숨어 마시던 칵테일의 주 재료이기도 했다.";
        Tags.Add(Define.CocktailTag.진);
    }
}

class Bmt_Rum : BaseMaterials
{
    public Bmt_Rum() : base(5)
    {
        Name = "화이트 럼";
        Info = "뱃사람들이 즐겨 마셨다고 하는 술. 사탕수수를 원료로 하며 감미로운 향과 독특한 풍미를 가지고 있다. 라임과 잘 어울린다.";
        Tags.Add(Define.CocktailTag.럼);
    }
}

class Bmt_Brandy : BaseMaterials
{
    public Bmt_Brandy() : base(6)
    {
        Name = "브랜디";
        Info = "와인을 증류하여 만든 술. 연금술사가 우연히 포도주를 증류한 것에서 유래하였다. 달콤한 맛과 향을 자랑한다.";
        Tags.Add(Define.CocktailTag.브랜디);
    }
}