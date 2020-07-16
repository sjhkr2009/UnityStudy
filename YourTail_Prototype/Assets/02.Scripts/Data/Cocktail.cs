using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CocktailName
{
    None = 0,
    TequillaSunrise = 1,
    TequillaTonic = 2,
    Margarita = 3,
    Paloma = 4,
    GodMother = 5,
    MoscowMule = 6,
    VodkaTonic = 7,
    BlackRussian = 8,
    Screwdriver = 9,
    Cosmopolitan = 10,
    GodFather = 11,
    Manhattan = 12,
    MintJulep = 13,
    WhiskeySour = 14,
    HighBall = 15,
    Negroni = 16,
    DryMartini = 17,
    Aviation = 18,
    GinTonic = 19,
    PinkLady = 20,
    Daiquiri = 21,
    Mojito = 22,
    Bacardi = 23,
    CubaLibre = 24
}

public class Cocktail
{
    public List<BaseMaterials> BaseMaterials { get; private set; } = new List<BaseMaterials>();
    public List<SubMaterials> SubMaterials { get; private set; } = new List<SubMaterials>();
    public List<string> BaseIDList { get; private set; } = new List<string>();
    public List<string> SubIDList { get; private set; } = new List<string>();
    public CocktailName cocktailName = CocktailName.None;
    public string Name_kr { get; private set; }
    public string Name_en { get; private set; }

    protected void SetName(string koreanName, string englishName)
    {
        Name_kr = koreanName;
        Name_en = englishName;
    }

    public Sprite image;

    protected void AddBase(BaseMaterials material)
    {
        BaseMaterials.Add(material);
        BaseIDList.Add(material.Id);
    }
    protected void AddSub(SubMaterials material)
    {
        SubMaterials.Add(material);
        SubIDList.Add(material.Id);
    }

    public int Proof { get; protected set; }
    public List<Define.CocktailTag> Tags { get; private set; } = new List<Define.CocktailTag>();
    public string Info { get; protected set; }

    protected void AddTag(Define.CocktailTag tag) => Tags.Add(tag);
    public List<string> GetTagToString()
    {
        List<string> tagList = new List<string>();
        
        foreach (Define.CocktailTag tag in Tags)
            tagList.Add(tag.ToString());

        return tagList;
    }
    public int GetProofGrade()
    {
        if (Proof <= 5) return 0;
        else if (Proof <= 10) return 1;
        else if (Proof <= 19) return 2;
        else if (Proof <= 30) return 3;
        else return 4;
    }

    public string Id { get; private set; }
    public void SetID(int code)
    {
        Id = "C" + code.ToString();
    }
    public Cocktail(int id)
    {
        SetID(id);
        image = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, id);
        cocktailName = (CocktailName)id;
    }
    public Cocktail()
    {
        SetID(0);
        image = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, 0);
        cocktailName = CocktailName.None;

        SetName("괴상한 음료", "Food Waste");

        Proof = Random.Range(0, (int)Define.CocktailMaxProof);
        Info = "뭔가 잘못된 것 같다.";
    }
}

class Ckt_TequillaSunrise : Cocktail
{
    public Ckt_TequillaSunrise() : base(1)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_OrangeJuice());
        AddSub(new Smt_GrenadineSyrup());

        cocktailName = CocktailName.TequillaSunrise;
        SetName("데킬라 선라이즈", "Tequilla Sunrise");

        Proof = 9;
        Info = "오렌지 주스 속에 가라앉은 그레나딘 시럽이 여명 속에서 먼동이 트길 기다리는 태양을 연상시킨다.";
    }
}

class Ckt_TequillaTonic : Cocktail
{
    public Ckt_TequillaTonic() : base(2)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_TonicWater());

        SetName("데킬라 토닉", "Tequilla Tonic");

        Proof = 15;
        Info = "진 토닉 대신 데킬라 토닉이라도 먹는다고? 아니야, 그 반대지.";
    }
}

class Ckt_Margarita : Cocktail
{
    public Ckt_Margarita() : base(3)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_OrangeLiqueur());

        SetName("마르가리타", "Margarita");

        Proof = 30;
        Info = "한 작가가 먼저 세상을 떠난 연인을 기리며 그녀의 이름을 담아 만든 사랑의 눈물";
    }
}
class Ckt_Paloma : Cocktail
{
    public Ckt_Paloma() : base(4)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_SodaWater());

        SetName("팔로마", "Paloma");

        Proof = 7;
        Info = "쌉싸름하면서 중독성 있는 멕시코의 정열이 입 안을 휘감는다.";
    }
}
class Ckt_GodMother : Cocktail
{
    public Ckt_GodMother() : base(5)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_Amaretto());

        SetName("갓 마더", "God Mother");

        Proof = 32;
        Info = "마더 로-씨아에서는 갓파더도 보드카를 만나 갓-마더가 된다!";
    }
}
class Ckt_MoscowMule : Cocktail
{
    public Ckt_MoscowMule() : base(6)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_GingerBeer());

        SetName("모스코 뮬", "Moscow Mule");

        Proof = 9;
        Info = "산뜻한 향과 깔끔한 목넘김, 보드카의 강렬함이 어우러진 술. 이름의 의미는 '모스크바의 노새'로 노새의 뒷발 킥과 같은 보드카의 신선한 자극을 상징한다.";
    }
}
class Ckt_VodkaTonic : Cocktail
{
    public Ckt_VodkaTonic() : base(7)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_TonicWater());

        SetName("보드카 토닉", "Vodka Tonic");

        Proof = 17;
        Info = "그냥 보드카에 물 타달라고 하지 그래? 아… 그것도 좋다고?";
    }
}
class Ckt_BlackRussian : Cocktail
{
    public Ckt_BlackRussian() : base(8)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_CoffeeLiqueur());

        SetName("블랙 러시안", "Black Russian");

        Proof = 24;
        Info = "부드러운 목넘김의 커피 리큐어로 보드카의 강렬함을 숨기고 달고 구수한 향을 냈다. 과음주의!";
    }
}
class Ckt_Screwdriver : Cocktail
{
    public Ckt_Screwdriver() : base(9)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_OrangeJuice());

        SetName("스크루드라이버", "Screwdriver");

        Proof = 10;
        Info = "과거 유전에서 일하던 노동자들이 드라이버로 섞어 마셨다고 한다. 오렌지 음료 맛이라 주스처럼 마시기 쉬워 '레이디 킬러'로 유명하니, 모르는 이성이 권한다면 의심해보자.";
    }
}
class Ckt_Cosmopolitan : Cocktail
{
    public Ckt_Cosmopolitan() : base(10)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_OrangeLiqueur());

        SetName("코스모폴리탄", "Cosmopolitan");

        Proof = 20;
        Info = "붉은 색감에 달콤한 향이 나는 화려한 칵테일. 그 이름답게 다양한 맛을 이 한 잔에 느낄 수 있다.";
    }
}
class Ckt_GodFather : Cocktail
{
    public Ckt_GodFather() : base(11)
    {
        AddBase(new Bmt_Whisky());
        AddSub(new Smt_Amaretto());

        SetName("갓 파더", "God Father");

        Proof = 32;
        Info = "대부의 인생은 첫 맛은 쓰게, 뒷 맛은 달게! 마피아를 그린 동명의 영화에서 유래한 칵테일로 이탈리아의 낭만을 담고 있다.";
    }
}
class Ckt_Manhattan : Cocktail
{
    public Ckt_Manhattan() : base(12)
    {
        AddBase(new Bmt_Whisky());
        AddSub(new Smt_Vermouth());

        SetName("맨해튼", "Manhattan");

        Proof = 27;
        Info = "전 세계가 사랑하는 달콤한 칵테일. 칵테일의 여왕이라고 불린다.";
    }
}
class Ckt_MintJulep : Cocktail
{
    public Ckt_MintJulep() : base(13)
    {
        AddBase(new Bmt_Whisky());
        AddSub(new Smt_Mint());
        AddSub(new Smt_Sugar());

        SetName("민트 줄렙", "Mint Julep");

        Proof = 25;
        Info = "여름에 상쾌한 이 한 잔을 맛본다면 민트초코를 싫어하는 사람도 잠시 돌아설 것이다. 술이 깰 때까지는…";
    }
}
class Ckt_WhiskeySour : Cocktail
{
    public Ckt_WhiskeySour() : base(14)
    {
        AddBase(new Bmt_Whisky());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_SodaWater());
        AddSub(new Smt_Sugar());

        SetName("위스키 사워", "Whiskey Sour");

        Proof = 15;
        Info = "과즙과 설탕으로 새콤달콤한 맛을 낸 위스키. 신맛과 단맛이 조화롭게 섞인 대표적인 칵테일이다.";
    }
}
class Ckt_HighBall : Cocktail
{
    public Ckt_HighBall() : base(15)
    {
        AddBase(new Bmt_Whisky());
        AddSub(new Smt_TonicWater());

        SetName("하이 볼", "High Ball");

        Proof = 13;
        Info = "단순히 위스키를 희석시킨 \'위스키 소다\'였지만, 하이볼이라는 별명으로 불린다. 골프장에서 높이 친 공이 위스키 소다를 마시던 사람 앞에 떨어진 것에서 유래했다는 이야기가 유명하다.";
    }
}
class Ckt_Negroni : Cocktail
{
    public Ckt_Negroni() : base(16)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_Vermouth());
        AddSub(new Smt_Campari());

        SetName("네그로니", "Negroni");

        Proof = 23;
        Info = "중후한 향에 캄파리의 쌉쌀함을 더한 어른의 풍미가 느껴지는 술. 미식가 네그로니 백작이 사랑한 정열의 한 잔.";
    }
}
class Ckt_DryMartini : Cocktail
{
    public Ckt_DryMartini() : base(17)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_Vermouth());

        SetName("드라이 마티니", "Dry Martini");

        Proof = 25;
        Info = "일반적으로 스위트한 다른 마티니와 달리 쓰고 강력한 맛을 내는 칵테일의 왕. 해밍웨이 작가가 즐겨 마신 술이다.";
    }
}
class Ckt_Aviation : Cocktail
{
    public Ckt_Aviation() : base(18)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_CherryLiqueur());

        SetName("애비에이션", "Aviation");

        Proof = 25;
        Info = "톡 쏘는 신맛 뒤의 은은한 단향. '비행'이라는 이름 그대로 석양이 지는 하늘을 날으는 짜릿함과 상쾌함을 준다.";
    }
}
class Ckt_GinTonic : Cocktail
{
    public Ckt_GinTonic() : base(19)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_TonicWater());

        SetName("진 토닉", "Gin Tonic");

        Proof = 10;
        Info = "토닉 워터가 진의 깊은 맛을 한 번 휘감아 깔끔함을 더한 기본 칵테일.";
    }
}
class Ckt_PinkLady : Cocktail
{
    public Ckt_PinkLady() : base(20)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_GrenadineSyrup());

        SetName("핑크 레이디", "Pink Lady");

        Proof = 30;
        Info = "여우주연상 주인공의 화려한 드레스를 연상시키는 연붉은색 칵테일. 영광스런 무대의 여배우에게 바치던 술이다.";
    }
}
class Ckt_Daiquiri : Cocktail
{
    public Ckt_Daiquiri() : base(21)
    {
        AddBase(new Bmt_Rum());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_Sugar());

        SetName("다이키리", "Daiquiri");

        Proof = 18;
        Info = "과일향으로 럼의 단맛을 한층 증폭시킨 칵테일. 과거 쿠바의 다이키리 광산 광부들이 럼에 라임을 넣어 갈증을 달랬다고 한다.";
    }
}
class Ckt_Mojito : Cocktail
{
    public Ckt_Mojito() : base(22)
    {
        AddBase(new Bmt_Rum());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_Mint());

        SetName("모히또", "Mojito");

        Proof = 9;
        Info = "럼의 독한 맛은 중화시키고 민트로 상큼함을 더했다. 16세기경 카리브 해의 해적들은 괴혈병 치료용으로 쓰던 기존의 라임주스 대신 이 모히또를 애용했다.";
    }
}
class Ckt_Bacardi : Cocktail
{
    public Ckt_Bacardi() : base(23)
    {
        AddBase(new Bmt_Rum());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_GrenadineSyrup());

        SetName("바카디", "Bacardi");

        Proof = 20;
        Info = "약간의 단맛과 쓴맛이 어우러진 칵테일. 미국에서 바텐더가 바카디에 바카디산 럼을 쓰지 않았다고 손님이 해당 바를 고소한 일화가 있다. 심지어 이겼다!";
    }
}
class Ckt_CubaLibre : Cocktail
{
    public Ckt_CubaLibre() : base(24)
    {
        AddBase(new Bmt_Rum());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_Cola());

        SetName("쿠바 리브레", "Cuba Libre");

        Proof = 9;
        Info = "럼과 콜라의 상쾌한 만남. 한 잔 마시면 쿠바 독립 전쟁에서 잠시 목을 축이던 병사의 상쾌한 기분을 느낄 수 있다.";
    }
}