using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    CubaLibre = 24,
    Gimlet = 25,
    OrangeBlossom = 26,
    GinDaisy = 27,
    GinBuck= 28,
    GinFizz = 29,
    CampariCocktail = 30,
    KissInTheDark = 31,
    WhiteLady = 32,
    Ambassador = 33,
    TequilaSunset = 34,
    Mexicola = 35,
    FrozenMargarita = 36,
    Sidecar = 37,
    AppleJack = 38,
    Olympic = 39,
    JackRose = 40,
    Classic = 41,
    FrenchConnection = 42,
    HavardCooler = 43,
    CafeRoyal = 44
}

public class Cocktail
{
	#region Stats

    public List<string> BaseIDList { get; private set; } = new List<string>();
    public List<string> SubIDList { get; private set; } = new List<string>();
    public CocktailName cocktailName = CocktailName.None;
    public string Name_kr { get; private set; }
    public string Name_en { get; private set; }
    public Sprite image;

    public int Proof { get; protected set; }
    public List<Define.CocktailTag> Tags { get; private set; } = new List<Define.CocktailTag>();
    public string Info { get; protected set; }
    public string Id { get; private set; }

    #endregion
    #region Derived Class Setting
    void SetID(int code)
    {
        Id = "C" + code.ToString();
    }
    protected void SetName(string koreanName, string englishName)
    {
        Name_kr = koreanName;
        Name_en = englishName;
    }
    protected void AddTag(List<Define.CocktailTag> tags)
    {
        foreach (Define.CocktailTag tag in tags)
            AddTag(tag);
    }
    protected void AddBase(BaseMaterials material)
    {
        BaseIDList.Add(material.Id);
        AddTag(material.Tags);
    }
    protected void AddSub(SubMaterials material)
    {
        SubIDList.Add(material.Id);
        AddTag(material.Tags);
    }

    protected void AddTag(Define.CocktailTag tag) 
    {
        if(!Tags.Contains(tag)) 
            Tags.Add(tag); 
    }
	#endregion
	#region Utility
    public List<string> GetTagToString()
    {
        List<string> tagList = new List<string>();
        
        foreach (Define.CocktailTag tag in Tags)
            tagList.Add(tag.ToString());

        return tagList;
    }
    public int GetProofGradeToInt() => (int)GetProofGrade();
    public Define.ProofGrade GetProofGrade()
    {
        for (int i = 0; i < Define.ProofGradeCriterion.Length; i++)
        {
            if (Proof <= Define.ProofGradeCriterion[i])
                return (Define.ProofGrade)i;
        }
        return (Define.ProofGrade)(Define.ProofGradeCriterion.Length - 1);
    }
	#endregion
	#region Constructor
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
	#endregion
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
        AddSub(new Smt_SodaWater());

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
        AddSub(new Smt_SodaWater());

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
        AddSub(new Smt_SodaWater());

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
        AddSub(new Smt_SodaWater());

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
class Ckt_Gimlet : Cocktail
{
    public Ckt_Gimlet() : base(25)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LimeJuice());

        SetName("김렛", "Gimlet");

        Proof = 33;
        Info = "깔끔한 진에 상쾌한 라임으로 하루의 상쾌함을 전해 주는 칵테일. 주당이 아니라면 딱 한 잔만 들도록 하자.";
    }
}
class Ckt_OrangeBlossom : Cocktail
{
    public Ckt_OrangeBlossom() : base(26)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_OrangeJuice());

        SetName("오렌지 블로섬", "Orange Blossom");

        Proof = 31;
        Info = "옛날 금주법 시행 때 몰래 술을 마시던 사람들은, 진의 냄새를 없애기 위해 오렌지를 사용했다고 한다.";
    }
}
class Ckt_GinDaisy : Cocktail
{
    public Ckt_GinDaisy() : base(27)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_GrenadineSyrup());

        SetName("진 데이지", "Gin Daisy");

        Proof = 16;
        Info = "데이지 꽃을 연상시키는 우아한 핑크색의 술. 은은한 과일향에 톡 쏘는 맛이 일품이다.";
    }
}
class Ckt_GinBuck : Cocktail
{
    public Ckt_GinBuck() : base(28)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_GingerBeer());

        SetName("진 벅", "Gin Buck");

        Proof = 16;
        Info = "탄산음료처럼 가볍고 상쾌한 맛을 자랑한며 입문자가 마시기에도 무난하다.";
    }
}
class Ckt_GinFizz : Cocktail
{
    public Ckt_GinFizz() : base(29)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_SodaWater());
        AddSub(new Smt_Sugar());

        SetName("진 피즈", "Gin Fizz");

        Proof = 16;
        Info = "달면서도 톡 쏘는 청량감을 가진 술. 소다수가 첨가되어 표면에서 기포 터지는 소리가 '피즈-' 와 비슷하다고 한다.";
    }
}
class Ckt_CampariCocktail : Cocktail
{
    public Ckt_CampariCocktail() : base(30)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_Campari());

        SetName("캄파리 칵테일", "Campari Cocktail");

        Proof = 36;
        Info = "캄파리 본연의 맛을 살린 칵테일. 다소 씁쓸한 맛이 약주를 마시는 느낌을 준다.";
    }
}
class Ckt_KissInTheDark : Cocktail
{
    public Ckt_KissInTheDark() : base(31)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_CherryLiqueur());
        AddSub(new Smt_Vermouth());

        SetName("키스 인 더 다크", "Kiss in the Dark");

        Proof = 30;
        Info = "진한 사랑을 담은 듯 어두운 붉은색의 칵테일. 몽환적인 단향에 농염한 맛이 난다. ";
    }
}
class Ckt_WhiteLady : Cocktail
{
    public Ckt_WhiteLady() : base(32)
    {
        AddBase(new Bmt_Gin());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_OrangeLiqueur());

        SetName("화이트 레이디", "White Lady");

        Proof = 34;
        Info = "흰 옷의 귀부인을 연상시키는 고급스러운 칵테일. 이름 덕분에 '핑크 레이디'와 함께 젊은 여성들 사이에서 인기가 많다.";
    }
}
class Ckt_Ambassador : Cocktail
{
    public Ckt_Ambassador() : base(33)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_OrangeJuice());
        AddSub(new Smt_Sugar());

        SetName("앰배서더", "Ambassador");

        Proof = 11;
        Info = "데킬라의 쌉쌀함을 단맛으로 중화시킨 술. 데킬라의 개성은 살리고 부담은 덜어 칵테일에 익숙하지 않은 사람도 무난하게 즐길 수 있다.";
    }
}
class Ckt_TequilaSunset : Cocktail
{
    public Ckt_TequilaSunset() : base(34)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_GrenadineSyrup());

        SetName("데킬라 선셋", "Tequila Sunset");

        Proof = 5;
        Info = "해질 무렵의 노을을 연상시키는 주홍빛의 칵테일. 레몬으로 상쾌한 맛을 더했으며 원래 레시피보다 데킬라 양을 줄여 저녁에 가볍게 한 잔 즐길 수 있도록 제조했다.";
    }
}
class Ckt_Mexicola : Cocktail
{
    public Ckt_Mexicola() : base(35)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_Cola());

        SetName("멕시콜라", "Mexicola");

        Proof = 12;
        Info = "멕시코의 데킬라에 미국의 콜라를 섞어 조화로운 맛을 낸 아메리카 화합의 장.";
    }
}
class Ckt_FrozenMargarita : Cocktail
{
    public Ckt_FrozenMargarita() : base(36)
    {
        AddBase(new Bmt_Tequilla());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_OrangeLiqueur());
        AddSub(new Smt_Sugar());

        SetName("프로즌 마르가리타", "Frozen Margarita");

        Proof = 10;
        Info = "알코올의 양을 덜어 부담은 줄이고, 얼음과 설탕으로 시원한 맛을 더한 마가리타. 푸른색 리큐어를 사용하여 보기만 해도 청량감이 느껴진다.";
    }
}

class Ckt_Sidecar : Cocktail
{
    public Ckt_Sidecar() : base(37)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_OrangeLiqueur());

        SetName("사이드카", "Sidecar");

        Proof = 30;
        Info = "브랜디의 풍부한 과일향에 새콤한 맛을 더한 칵테일. 강한 향과 상쾌한 뒷맛으로 애주가들에게 사랑받는다.";
    }
}
class Ckt_AppleJack : Cocktail
{
    public Ckt_AppleJack() : base(38)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_GrenadineSyrup());

        SetName("애플 잭", "Apple Jack");

        Proof = 20;
        Info = "300년의 역사를 가진 미국의 전통주. 당시 흔했던 사과를 이용한 원시적인 형태의 증류주이다.";
    }
}
class Ckt_Olympic : Cocktail
{
    public Ckt_Olympic() : base(39)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_OrangeJuice());
        AddSub(new Smt_OrangeLiqueur());

        SetName("올림픽", "Olympic");

        Proof = 20;
        Info = "1900년 파리올림픽을 기념하여 만든 칵테일로, 브랜디의 과일향에 오렌지가 어우러진 감칠맛이 일품이다.";
    }
}
class Ckt_JackRose : Cocktail
{
    public Ckt_JackRose() : base(40)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_GrenadineSyrup());

        SetName("잭 로즈", "Jack Rose");

        Proof = 20;
        Info = "애플 잭에서 파생된 칵테일. 이름답게 아름다운 장미향을 띠고 있어 낭만적인 분위기를 풍긴다.";
    }
}

/// <summary>
/// [사용되지 않음]
/// </summary>
class Ckt_Classic : Cocktail
{
    public Ckt_Classic() : base(41)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_OrangeLiqueur());
        AddSub(new Smt_CherryLiqueur());

        SetName("클래식", "Classic");

        Proof = 26;
        Info = "고풍스러운 맛을 자랑하는 과일향 칵테일. 다양한 재료가 들어가지만 서로 잘 어우러져 조화로운 진한 맛을 보여준다.";
    }
}
class Ckt_FrenchConnection : Cocktail
{
    public Ckt_FrenchConnection() : base(42)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_Amaretto());

        SetName("프렌치 커넥션", "French Connection");

        Proof = 32;
        Info = "오랜 숙성을 거친 프랑스 코냑 지방의 브랜디를 사용한 칵테일. 갓 파더의 브랜디 버전으로, 무거운 스카치 위스키 대신 브랜디의 달콤하고 낭만적인 향을 살렸다.";
    }
}
class Ckt_HavardCooler : Cocktail
{
    public Ckt_HavardCooler() : base(43)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_SodaWater());
        AddSub(new Smt_Sugar());

        SetName("하버드 쿨러", "Havard Cooler");

        Proof = 12;
        Info = "사과향 브랜디에 레몬과 탄산이 어우러져 새콤달콤한 과일향 음료같은 맛을 낸다.";
    }
}
class Ckt_CafeRoyal : Cocktail
{
    public Ckt_CafeRoyal() : base(44)
    {
        AddBase(new Bmt_Brandy());
        AddSub(new Smt_CoffeeLiqueur());
        AddSub(new Smt_Sugar());

        SetName("카페 로얄", "Café Royal");

        Proof = 20;
        Info = "왕족의 커피라는 이름에 걸맞은 기품을 가진 따뜻한 커피향 칵테일. 나폴레옹이 즐겨 마셨다는 설이 있다.ㅌ";
    }
}