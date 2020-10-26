using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMaterials : CocktailMaterials
{
    public SubMaterials(int id) : base(MaterialType.Sub)
    {
        SetID(id);
        image = GameManager.Resource.LoadImage(Define.ImageType.Sub, id);
    }
    public SubMaterials() : base(MaterialType.Sub)
    {
        SetID(0);
    }
    protected void SetTag(Define.CocktailTag tag1, Define.CocktailTag tag2 = Define.CocktailTag.Null, Define.CocktailTag tag3 = Define.CocktailTag.Null, Define.CocktailTag tag4 = Define.CocktailTag.Null)
    {
        Tags.Add(tag1);
        if (tag2 != Define.CocktailTag.Null) Tags.Add(tag2);
        else return;
        if (tag3 != Define.CocktailTag.Null) Tags.Add(tag3);
        else return;
        if (tag4 != Define.CocktailTag.Null) Tags.Add(tag4);
    }
}

class Smt_OrangeJuice : SubMaterials
{
    public Smt_OrangeJuice() : base(1)
    {
        Name = "오렌지 주스";
        Info = "달콤한 향과 풍부한 과즙으로 전세계에서 사랑받는 과일로 만든 주스";
        SetTag(Define.CocktailTag.오렌지, Define.CocktailTag.주스, Define.CocktailTag.과일);
    }

}

class Smt_LimeJuice : SubMaterials
{
    public Smt_LimeJuice() : base(2)
    {
        Name = "라임 주스";
        Info = "강한 신맛에 더해 레몬보다 씁쓸한 뒷맛이 느껴진다.";
        SetTag(Define.CocktailTag.라임, Define.CocktailTag.주스, Define.CocktailTag.과일);
    }
}

class Smt_LemonJuice : SubMaterials
{
    public Smt_LemonJuice() : base(3)
    {
        Name = "레몬 주스";
        Info = "미각이 마비된 사람들은 가끔 레몬을 액체로만 접하는 것이 아쉬워 생레몬 먹기 대회를 연다고 한다.";
        SetTag(Define.CocktailTag.레몬, Define.CocktailTag.주스, Define.CocktailTag.과일);
    }
}

class Smt_GrenadineSyrup : SubMaterials
{
    public Smt_GrenadineSyrup() : base(4)
    {
        Name = "그레나딘 시럽";
        Info = "강한 붉은빛을 띠는 석류 시럽. 다량의 당분이 들어가 무겁기 때문에 칵테일 바닥에 가라앉아 붉은 층을 이룬다.";
        SetTag(Define.CocktailTag.석류, Define.CocktailTag.시럽, Define.CocktailTag.과일);
    }
}

/// <summary>
/// [사용되지 않음] 소다수로 대체 (Smt_SodaWater)
/// </summary>
class Smt_TonicWater : SubMaterials
{
    public Smt_TonicWater() : base(5)
    {
        Name = "토닉 워터";
        Info = "그런 이름의 재료는 없습니다. 자신이 '토닉 워터'라는 재료를 보면 즉시 자리를 피하시고 개발자에게 알리십시오.";
        SetTag(Define.CocktailTag.탄산);
    }
}

class Smt_OrangeLiqueur : SubMaterials
{
    public Smt_OrangeLiqueur() : base(6)
    {
        Name = "오렌지 리큐어";
        Info = "오렌지 특유의 단맛과 주홍빛이 가미된 혼합주";
        SetTag(Define.CocktailTag.오렌지, Define.CocktailTag.과일, Define.CocktailTag.리큐어);
    }
}

class Smt_SodaWater : SubMaterials
{
    public Smt_SodaWater() : base(7)
    {
        Name = "소다수";
        Info = "증류수에 이산화탄소를 섞어 청량감을 부여하기 위해 사용하는 탄산수";
        SetTag(Define.CocktailTag.탄산);
    }
}

class Smt_Amaretto : SubMaterials
{
    public Smt_Amaretto() : base(8)
    {
        Name = "아마레또";
        Info = "아몬드 향에 달콤한 맛이 나는 씨앗 계열 리큐어의 일종. 아몬드에 더해 살구나 복숭아 씨를 혼합하기도 한다.";
        SetTag(Define.CocktailTag.아몬드, Define.CocktailTag.리큐어);
    }
}

class Smt_GingerBeer : SubMaterials
{
    public Smt_GingerBeer() : base(9)
    {
        Name = "진저 에일";
        Info = "생강과 매콤한 향의 재료들을 섞어 캐러멜 색으로 착색한 무알콜 음료";
        SetTag(Define.CocktailTag.생강, Define.CocktailTag.탄산, Define.CocktailTag.주류);
    }
}
class Smt_CoffeeLiqueur : SubMaterials
{
    public Smt_CoffeeLiqueur() : base(10)
    {
        Name = "커피 리큐어";
        Info = "당분과 커피향으로 부드러운 맛을 낸 혼합주";
        SetTag(Define.CocktailTag.커피, Define.CocktailTag.리큐어);
    }
}
class Smt_Vermouth : SubMaterials
{
    public Smt_Vermouth() : base(11)
    {
        Name = "베르무트";
        Info = "와인에 당분을 섞고 각종 향료로 강한 향미를 더한 고농도의 혼성주";
        SetTag(Define.CocktailTag.주류, Define.CocktailTag.베르무트);
    }
}
class Smt_Mint : SubMaterials
{
    public Smt_Mint() : base(12)
    {
        Name = "민트 잎";
        Info = "특유의 상쾌한 향이 매우 강해서 향을 내기 위한 재료로 사용된다. 가끔 뜬금없는 음식에서 발견되어 사람들의 공분을 산다.";
        SetTag(Define.CocktailTag.민트);
    }
}
class Smt_Sugar : SubMaterials
{
    public Smt_Sugar() : base(13)
    {
        Name = "설탕";
        Info = "단맛을 가진 대표적인 감미료. 백종원이 별로 좋아하지는 않는다고 말하면서 자주 사용한다.";
        SetTag(Define.CocktailTag.설탕);
    }
}
class Smt_Campari : SubMaterials
{
    public Smt_Campari() : base(14)
    {
        Name = "캄파리";
        Info = "붉은빛을 띠는 혼합주의 일종으로, 약용 술로 쓰이던 것을 개량해서 만들었다고 한다.";
        SetTag(Define.CocktailTag.주류, Define.CocktailTag.캄파리);
    }
}
class Smt_CherryLiqueur : SubMaterials
{
    public Smt_CherryLiqueur() : base(15)
    {
        Name = "체리 리큐어";
        Info = "체리에 설탕시럽을 첨가하여 강한 단맛이 나는 과일 혼합주";
        SetTag(Define.CocktailTag.체리, Define.CocktailTag.리큐어, Define.CocktailTag.과일);
    }
}
class Smt_Cola : SubMaterials
{
    public Smt_Cola() : base(16)
    {
        Name = "콜라";
        Info = "누군가에게는 코카콜라인지 펩시콜라인지가 아주 중요한 문제일지도 모른다.";
        SetTag(Define.CocktailTag.탄산, Define.CocktailTag.음료수, Define.CocktailTag.콜라);
    }
}