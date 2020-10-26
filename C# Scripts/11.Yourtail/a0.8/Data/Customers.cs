using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    #region 기본 정보
    public string Name { get; protected set; }
    public string ID { get; private set; }
    public Define.CustomerType CustomerType { get; private set; }
    public Sprite Image { get; set; }
    public bool IsActive { get; set; }
    #endregion

    #region 호감도
    private int _level = 1;
    public int Level
    {
        get => _level;
        set { _level = Mathf.Clamp(value, 1, MaxLevel); }
    }
    public int MaxLevel { get; private set; }
    protected void SetMaxLevel(int value)
    {
        MaxLevel = Mathf.Clamp(value, 1, Define.MaxLevel);
        Level = 1;
        Exp = 0;
    }
    private int _exp;
    public int Exp
    {
        get => _exp;
        set
        {
            if(value > Define.RequiredEXP[_level])
            {
                Level++;
                GameManager.Data.levelUp = true;
                Exp = (value - Define.RequiredEXP[_level]);
            }
            else
            {
                _exp = value;
            }
        }
    }
    #endregion

    #region 오더 관련
    protected Dictionary<int, List<Order>> wishlist = new Dictionary<int, List<Order>>();
    private int _currentOrderIndex;
    public int CurrentOrderIndex
    {
        get => _currentOrderIndex;
        set
        {
            if (value == 0 || wishlist[Level].Count == 0) _currentOrderIndex = 0;
            else _currentOrderIndex = (value % wishlist[Level].Count);
        }
    }
    protected void SetWishlist(int maxLevel)
    {
        for (int i = 1; i <= maxLevel; i++)
            wishlist.Add(i, new List<Order>());
    }
    private Order currentOrder = null;
    public Order GetOrder()
    {
        if (currentOrder == null)
            currentOrder = wishlist[Level][CurrentOrderIndex++];

        return currentOrder;
    }
    public Order GetRandomOrder(int level = int.MaxValue)
    {
        if (level == int.MaxValue)
            level = Level;
        
        if (currentOrder == null)
            currentOrder = wishlist[Mathf.Clamp(level, 1, 4)][Random.Range(0, wishlist[Level].Count)];

        return currentOrder;
    }
    /// <summary>
    /// 현재 레벨 이하의 주문들을 랜덤으로 가져옵니다. n번째 인자는 현재 레벨보다 n - 1 만큼 낮은 티어의 주문을 받을 확률을 의미합니다. 최대/최소티어를 초과하지 않습니다.
    /// 예를 들어, 내 레벨이 3일 때 70, 50, 30, 10을 입력할 경우, 3, 2, 1, 0티어 주문을 7:5:3:1의 확률로 랜덤하게 가져옵니다. 단, 0티어 주문은 없으므로 1티어에 합산되어 최종적으로 7:5:4의 확률을 갖습니다.
    /// </summary>
    /// <param name="rateToCurrent"></param>
    /// <returns></returns>
    public Order GetOrderInLow(params float[] rateToCurrent)
    {
        int targetLevel = Mathf.Clamp(Level - rateToCurrent.RandomInWeighted(), 1, 4);
        return GetRandomOrder(targetLevel);
    }
    public void ResetOrder()
    {
        currentOrder = null;
    }
    protected void SetOrder(int level, string orderContents, List<CocktailName> requiredCocktail = null, 
        List<Define.ProofGrade> requiredProofGrade = null, List<Define.CocktailTag> requiredTags = null,
        List<Define.ProofGrade> avoidProofGrade = null, List<Define.CocktailTag> avoidTags = null)
    {
        Order order = new Order();
        order.requiredCocktail = requiredCocktail;
        order.requiredProofGrade = requiredProofGrade;
        order.requiredTag = requiredTags;
        order.avoidProofGrade = avoidProofGrade;
        order.orderContents = orderContents;
        order.avoidTag = avoidTags;
        order.CustomerName = Name;

        if (!wishlist.ContainsKey(level)) wishlist.Add(level, new List<Order>());
        wishlist[level].Add(order);
    }
    #endregion

    #region 기타 스크립트
    
    // 칵테일을 제공했을 때 새가 반응하는 대사가 들어 있습니다. Key값 -1, 0, 1에 각각 BAD, SOSO, GOOD일 경우의 대사가 있습니다.
    public Dictionary<Define.Reaction, string> ReactionSctipt { get; private set; } = new Dictionary<Define.Reaction, string>();
    protected void SetReactionSctipt(string good, string soso, string bad)
    {
        ReactionSctipt.Add(Define.Reaction.BAD, bad);
        ReactionSctipt.Add(Define.Reaction.SOSO, soso);
        ReactionSctipt.Add(Define.Reaction.GOOD, good);
    }
    public string Liking { get; protected set; }

    #endregion

    public Customers(int index)
    {
        CurrentOrderIndex = 0;
        IsActive = true;
        CustomerType = (Define.CustomerType)index;
        ID = $"CT{index}";
        Image = GameManager.Resource.LoadImage(Define.ImageType.Customer, index);
        Level = 1;
        SetMaxLevel(Define.BirdMaxLevel[index]);
        Exp = 0;
    }
}

public class Eagle : Customers
{
    public Eagle() : base(1)
    {
        Name = "머머리 독수리";
        Liking = "(임시) 칵테일바가 낯선 어르신으로, 평소엔 도수만 맞춰서 마시지만 가끔씩은 칵테일에 관심을 보입니다.";

        #region 1단계 주문
        SetOrder(1, "제일 독한걸로 하나 줘 보게. 요즘 술들은 술 같지가 않아.",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        SetOrder(1, "알록달록한 건 됐으니께 소주 한 잔 주쇼. 왜 그 손주놈이 마트에서 토닉 뭐시기 사 와서 비스무리하게 만들더만, 그거 있잖수?",
            requiredCocktail: new List<CocktailName>() { CocktailName.GinTonic });
        SetOrder(1, "마시고 취할만한 걸로 하나 주시게. 옛날 소주정도면 딱 좋겠군.",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈 });
        SetOrder(1, "오늘은 좀 피곤하구만. 맥주같이 가벼운걸로 한 잔 하지.",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함, Define.ProofGrade.매우약함 });
        SetOrder(1, "요즘 젊은이들이 마시는걸로 하나 주시게나. 독하지도 맹하지도 않은... 그 왜 옛날 소주랑 맥주 중간쯤되는 시원섭섭한거 말일세. 무슨 맛인지 궁금하군.",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.중간 });
        #endregion
        #region 2단계 주문
        SetOrder(2, "내가 왕년엔 꽤 잘 나갔다~ 이 말이야!",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        SetOrder(2, "나 때는 말이야, 술에 주스 같은건 넣을 생각도 안 했어!",
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스 });
        SetOrder(2, "어렸을 적에 과수원의 그 향기... 그리워지는구만.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 });
        SetOrder(2, "내가 젊었을 적에... 행사 기념으로 만들어진 칵테일이 있었는데 말이야... 그게 뭐더라...",
            requiredCocktail: new List<CocktailName>() { CocktailName.Olympic });
        SetOrder(2, "손주놈이 또 이상한걸 만들던데... 럼에 이것저것 섞던데 뭐던 줘보슈.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.럼 });
        #endregion
        #region 3단계 주문
        SetOrder(3, "오늘은 위스키가 땡기는구만. 독한 걸로 부탁하네.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.위스키 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈, Define.ProofGrade.셈 });
        SetOrder(3, "요즘 젊은이들은 술에다가 탄산음료 같은 것도 섞는다고 들었는데 말이야.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산, Define.CocktailTag.음료수 });
        SetOrder(3, "내가 젊었을 때 미국에서 마셨던 그 술... 아직도 잊을 수가 없구만!",
            requiredCocktail: new List<CocktailName>() { CocktailName.AppleJack });
        SetOrder(3, "약주랑 비슷한 쓴 맛이 나는 술이 있다고 들었는데... 그걸로 주시게.",
            requiredCocktail: new List<CocktailName>() { CocktailName.CampariCocktail });
        SetOrder(3, "내가 왕년에 러시아에 있었을 때... 꽤나 독한 술이 있었단 말이야. 그게 먹어보고 싶은데...",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.보드카 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈, Define.ProofGrade.매우셈 });
        #endregion
        #region 4단계 주문
        SetOrder(4, "어른이 굳이 말 안 해도 알아서 가져오란 말이야!",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈 });
        SetOrder(4, "옛날이 그립구먼... 그때가 좋았는데.",
            requiredCocktail: new List<CocktailName>() { CocktailName.Negroni });
        SetOrder(4, "요즘 애들은 말이야... 술이 독하다고 사이다 같은거나 섞고 말이야! 에잉 쯧쯔...",
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 },
            avoidProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(4, "어떤 젊은 친구가 여기서 보라색 술을 하나 마시던데... 그게 나도 마시고 싶구먼.",
            requiredCocktail: new List<CocktailName>() { CocktailName.Aviation });
        SetOrder(4, "독한 걸로.",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        #endregion

        SetReactionSctipt("자네..실력이 대단하군..그려...", "오호..이것도 먹을만 하군....", "으른을...놀리는겐가...?");
    }
}

public class Parrot : Customers
{
    public Parrot() : base(2)
    {
        Name = "앵무새";
        Liking = "(임시) 기분에 따라 주문하는 경우가 많습니다.";

        #region 1단계 주문
        SetOrder(1, "오렌지 나무에서 나는 향이 맡고 싶어요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.오렌지, Define.CocktailTag.주스 });
        SetOrder(1, "\'키스 인 더 다크\'라는 칵테일이 있다던데. 이름이 참 예쁘지 않아요?",
            requiredCocktail: new List<CocktailName>() { CocktailName.KissInTheDark });
        SetOrder(1, "제 깃털처럼 새빨간 과일이 들어간 걸로 한 잔 주세요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.진, Define.CocktailTag.석류 });
        SetOrder(1, "입이 좀 텁텁한데 청량감 있는 칵테일은 없을까요? 톡 쏘는 맛이라든가.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 });
        SetOrder(1, "그 왜... 레몬이랑 비슷한 거... 아, 맞아 라임! 그게 들어간 칵테일이 마시고 싶어요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.라임, Define.CocktailTag.주스 });
        #endregion
        #region 2단계 주문
        SetOrder(2, "상쾌한 향이 곁들여져있는 칵테일이 마시고 싶어요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.민트 });
        SetOrder(2, "꿈 속을 걷는 듯한 기분의 칵테일이 마시고 싶네요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.중간 });
        SetOrder(2, "낭만적인 분위기의 칵테일 어디 없을까요? 장미 향이 나는 칵테일이 있다고 들었는데...",
            requiredCocktail: new List<CocktailName>() { CocktailName.JackRose });
        SetOrder(2, "토닉 워터가 들어간 무던한 스타일의 칵테일로 부탁드려요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 },
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스 });
        SetOrder(2, "핑크빛을 띄는 칵테일이 있다면서요? 그걸로 하나 부탁해요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.GinDaisy });
        #endregion
        #region 3단계 주문
        SetOrder(3, "톡톡 튀는 거!",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 });
        SetOrder(3, "오늘은 과일향이 가득한 칵테일이 마시고 싶어요. 이왕이면 시큼한 쪽으로!",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일, Define.CocktailTag.라임, Define.CocktailTag.레몬 });
        SetOrder(3, "하늘을 나는 기분의 칵테일이 마시고 싶어요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.Aviation });
        SetOrder(3, "오늘은 좀 졸리네요. 기운을 차릴 수 있을만한걸 마시고 싶네요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.커피, Define.CocktailTag.과일 });
        SetOrder(3, "주스가 듬뿍듬뿍~ 시럽도 듬뿍듬뿍~ 뭔지 알겠어요?",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스, Define.CocktailTag.시럽 });
        #endregion
        #region 4단계 주문
        SetOrder(4, "과일이 안 들어간 달짝지근한 칵테일이 먹고 싶어요~",
            requiredCocktail: new List<CocktailName>() { CocktailName.CafeRoyal, CocktailName.MintJulep },
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.설탕 },
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 });
        SetOrder(4, "보드카는 먹고 싶은데 도수가 너무 강해서 고민이에요. 뭔가 방법이 없을까요?",
            requiredCocktail: new List<CocktailName>() { CocktailName.MoscowMule },
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.보드카 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(4, "요즘 날이 자칫 잘못하면 감기 걸리기 십상이더라구요~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.생강, Define.CocktailTag.탄산, Define.CocktailTag.주류 });
        SetOrder(4, "이 바에서 제일 독한 것... 오늘은 그런걸 마셔보고 싶네요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.WhiteLady },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        SetOrder(4, "오렌지 리큐어에 레몬 주스를 넣은 칵테일... 베이스가 뭐였는지는 확실히 기억 안 나는데, 아무튼 그걸로 부탁드려요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.Sidecar, CocktailName.WhiteLady },
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.오렌지, Define.CocktailTag.과일, Define.CocktailTag.레몬 });
        #endregion

        SetReactionSctipt("음- 너무 맛있어요!", "뭔가 살짝 아쉬운데.. 그래도 고마워요.", "제가 찾던 건 아니네요.");
    }
}

public class Flamingo : Customers
{
    public Flamingo() : base(3)
    {
        Name = "홍학";
        Liking = "(임시) 요구사항은 대부분 간단하지만 표현력이 풍부합니다.";

        #region 1단계 주문
        SetOrder(1, "강하면서 상큼한 칵테일 어디 없을까?나는 레몬보단 라임을 좋아해^^",
            requiredCocktail: new List<CocktailName>() { CocktailName.Gimlet });
        SetOrder(1, "상콤상콤한 칵테일이 먹고싶어ㅎㅎ 과일이 2가지 들어가면 좋겠어!",
            requiredCocktail: new List<CocktailName>() { CocktailName.Aviation });
        SetOrder(1, "달달한 과일이 죠아 ㅎㅎ 그런데 달달하기만 하면 재미없지~ 도수가 세면 좋겠어!",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈, Define.ProofGrade.매우셈 });
        SetOrder(1, "향이 강한 칵테일 없을까? 알싸한 느낌으로다가~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.생강 });
        SetOrder(1, "아이셔~아이셔~아이셔~ 신맛에 중독됐나봐 >.< ",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬 });
        #endregion
        #region 2단계 주문
        SetOrder(2, "과일 중에 그거 있잖아 그거... 그거 넣어서 하나 줘 ^^",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.체리 });
        SetOrder(2, "오늘은 톡톡 쏘는 맛의 칵테일이 먹고 싶네 >_<",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 });
        SetOrder(2, "세상엔 이런 과일~ 저런 과일~ 맛있는 과일들이 너무 많아~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 });
        SetOrder(2, "오늘은 카페인이 좀 필요한 날이야 ㅠㅠ",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.커피 });
        SetOrder(2, "다들 내가 술을 잘 마신다고 부러워하더라구~ 후훗^^",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        #endregion
        #region 3단계 주문
        SetOrder(3, "아니 글쎄~ 어떤 녀석이 민트초코가 좋다고 그러더라고! 그 친구랑은 절교할까 생각 중이야!",
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.민트 });
        SetOrder(3, "오늘은 기분 안 좋은 일이 있었어 ㅜㅜ 도수 센 칵테일 하나 줘!",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈 });
        SetOrder(3, "레몬에 설탕, 탄산까지 섞어서... 맛있게 한 잔 부탁할게 ^^",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬, Define.CocktailTag.설탕, Define.CocktailTag.탄산 });
        SetOrder(3, "콜라 들어가는 칵테일이 있었는데, 주스도 들어갔던가..? 아무튼 그걸로 부탁할게 ^^",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.라임, Define.CocktailTag.주스, Define.CocktailTag.콜라 },
            requiredProofGrade: new List<Define.ProofGrade>() { });
        SetOrder(3, "엄청 쎈 것도 별로고, 약한 것도 별로고, 주스 들어간 것도 별로고... 아무튼 그렇게 부탁해~",
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스 },
            avoidProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈, Define.ProofGrade.약함 });
        #endregion
        #region 4단계 주문
        SetOrder(4, "음~ 오늘 내가 뭐 마시고 싶은지 적당히 맞춰 봐~^^",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스 });
        SetOrder(4, "과일주스를 싫어하는 건 아니지만… 오늘은 그런거 마시러 온 건 아냐.",
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.주스, Define.CocktailTag.과일 });
        SetOrder(4, "독하지 않은 걸로 부탁해~",
            avoidProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈, Define.ProofGrade.매우셈 });
        SetOrder(4, "저번에 내가 어떤게 싫다고 했는지 기억해? 그 대신에 탄산을 넣으면 맛좋은 칵테일이 된다고 들었어 ^^",
            requiredCocktail: new List<CocktailName>() { CocktailName.HighBall },
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 },
            avoidTags: new List<Define.CocktailTag>() { Define.CocktailTag.민트 });
        SetOrder(4, "저번에 내가 중독됐다고 했던 과일 있잖아~ 거기에 그레나딘 시럽을 넣은 칵테일 부탁해~ >.<",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬, Define.CocktailTag.석류, Define.CocktailTag.시럽, Define.CocktailTag.과일 });
        #endregion

        SetReactionSctipt("우왕 너무 맛있다 ㅎㅎ", "나쁘지 않은걸~?!", "흐음..이건 뭐지?");
    }
}

public class Swan : Customers
{
    public Swan() : base(4)
    {
        Name = "고니";
        Liking = "(임시) 칵테일명으로 주문하거나, 특정 칵테일을 요구하는 경우가 많습니다.";

		#region 1단계 주문
		SetOrder(1, "얼마전 친구의 웨딩드레스에 꽂을 '오렌지 꽃'을 선물했어요. 그 꽃을 추억하고 싶어요. 오렌지 블로섬 하나 주세요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.OrangeBlossom });
        SetOrder(1, "드라이하고 강한 칵테일 없을까? 추천 부탁드려요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.DryMartini });
        SetOrder(1, "\'캄파리\'라는 술이 있는 것 아세요? 이 술을 넣어서도 칵테일을 만들 수 있다네요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.캄파리 });
        SetOrder(1, "리큐어가 뭔지 혹시 아시나요? 여러 가지 과일, 향료 등을 첨가한 혼합주래요. 그걸 마셔보고 싶어요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.리큐어 });
        SetOrder(1, "오늘는 진이 좋겠다! \'진 벅\' 한 잔 주세요~",
            requiredCocktail: new List<CocktailName>() { CocktailName.GinBuck });
        #endregion
        #region 2단계 주문
        SetOrder(2, "\'스크루 드라이버\'라는 칵테일이 있다고 들었어요. 그걸 한 번 마셔보고 싶네요~",
            requiredCocktail: new List<CocktailName>() { CocktailName.Screwdriver });
        SetOrder(2, "\'칵테일의 여왕\'이라고 불리는 칵테일이 있다고 하네요. 뭔지 정말 궁금하네~",
            requiredCocktail: new List<CocktailName>() { CocktailName.Manhattan });
        SetOrder(2, "고전 영화인 \'대부\'를 혹시 보셨나요? 오늘 보고 왔는데 정말 인상 깊더라구요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.GodFather });
        SetOrder(2, "\'왕족의 커피\'라는 별명을 가진 칵테일이 있대요. 마셔보고 싶다~",
            requiredCocktail: new List<CocktailName>() { CocktailName.CafeRoyal });
        SetOrder(2, "사람들은 누구나 마음 속에 핑크색을 품고있어요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.PinkLady });
        #endregion
        #region 3단계 주문
        SetOrder(3, "붉고 정열적인 칵테일로 부탁드릴게요. 아! 석류는 별로에요.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.체리, Define.CocktailTag.리큐어 });
        SetOrder(3, "과거에 여배우들을 위한 칵테일이 있었다고 하네요. 이름도 뭔가 그런 느낌이었는데~",
            requiredCocktail: new List<CocktailName>() { CocktailName.PinkLady });
        SetOrder(3, "와인에 이것저것 넣은 술이 있다고 하네요. 그 술로 만든 칵테일이 먹어보고 싶어요~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.베르무트 });
        SetOrder(3, "골프에서 이름이 유래되었다는 칵테일을 들었는데... 그걸로 부탁드려요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.HighBall });
        SetOrder(3, "칵테일에서 나는 소리가 그 이름이 되었다는 칵테일이 있나봐요!",
            requiredCocktail: new List<CocktailName>() { CocktailName.GinFizz });
        #endregion
        #region 4단계 주문
        SetOrder(4, "러시아의 한 도시 이름을 따서 이름이 지어진 칵테일이 있다고 들었는데...",
            requiredCocktail: new List<CocktailName>() { CocktailName.MoscowMule });
        SetOrder(4, "\'갓 파더\' 칵테일의 베이스를 바꾸면 다른 칵테일이 된다고 들었어요. 무슨 맛일까 궁금하네요.",
            requiredCocktail: new List<CocktailName>() { CocktailName.GodMother, CocktailName.FrenchConnection },
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.아몬드 });
        SetOrder(4, "새콤달콤한 맛이 나는 그런 칵테일... 어디 없을까요?",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일, Define.CocktailTag.설탕, Define.CocktailTag.주스 });
        SetOrder(4, "진한 사랑을 표현한 칵테일이 있다는 것 아세요? 정말 낭만적이죠~",
            requiredCocktail: new List<CocktailName>() { CocktailName.KissInTheDark });
        SetOrder(4, "정말 가볍게 마실 수 있는 칵테일 하나 부탁드릴게요~",
            requiredCocktail: new List<CocktailName>() { CocktailName.TequilaSunset },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우약함, Define.ProofGrade.약함 });
        #endregion

        SetReactionSctipt("맛이 좋은걸요~!^^", "이것도 나름 괜찮네요~!", "다시 만들어 주면 좋겠어요..ㅠ");
    }
}

public class Penguin : Customers
{
    public Penguin() : base(5)
    {
        Name = "펭귄";
        Liking = "(임시) 칵테일 이름은 잘 모르지만, 여러 가지 식물을 잘 알고 있습니다.";

        #region 1단계 주문
        SetOrder(1, "\'댁길라\'라는 술이 들어간 칵테일이 맛있다고 들었는데... 그게 먹어보고 싶어요오. 약하게 해주세요오.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.데킬라 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(1, "과일 들어간거면 아무거나 괜찮을거 같아요오.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 });
        SetOrder(1, "과일 들어간거요오! 컨디션은 애매하니 좀 약한걸로...",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함, Define.ProofGrade.매우약함 });
        SetOrder(1, "\'앱에이숑\'이라는 칵테일이 있다고 들었는데 그게 먹어보고 싶어요오!",
            requiredCocktail: new List<CocktailName>() { CocktailName.Aviation },
            requiredTags: new List<Define.CocktailTag>() { },
            requiredProofGrade: new List<Define.ProofGrade>() { });
        SetOrder(1, "과일 들어간 적당한 칵테일이 마시고 싶어요오.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 },
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.중간 });
        #endregion
        #region 2단계 주문
        SetOrder(2, "저는 어렸을 때부터 풀떼기들에 관심이 많았단 말이죠오.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.민트 });
        SetOrder(2, "아몬드의 생태에 대해 아시나요오? 아몬드는 원래 청산가리가 있지만, 독이 없는 돌연변이 품종이 재배되면서 우리가 먹는 식용 아몬드가 되었답니다아.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.아몬드 });
        SetOrder(2, "체리가 벚나무의 열매라는건 혹시 알고 계시나요오? 봄에 예쁜 벚꽃이 피는 벚나무에서 피는 체리를 상상하기는 쉽지 않긴 하죠오.",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.체리 });
        SetOrder(2, "데이지 꽃은 이탈리아의 국화로, 태양을 따라 피고 짐이 바뀌어서 '태양의 눈'이라는 별명이 있다고 합니다아. 아무튼 그렇대요오.",
            requiredCocktail: new List<CocktailName>() { CocktailName.GinDaisy });
        SetOrder(2, "생강은 예로부터 동서양 양 쪽에서 인기가 많은 식용 식물이었어요오. 진저브레드 뭔지 아시죠오?",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.생강 });
        #endregion
        #region 3단계 주문
        SetOrder(3, "라임과 레몬이 둘 다 신 과일의 대명사로 불리지만, 먹어보면 분명히 차이가 있어요오. 오늘은 약간 씁쓸한 쪽이 당기네요오~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.라임 });
        SetOrder(3, "라임과 레몬이 둘 다 신 과일의 대명사로 불리지만, 먹어보면 분명히 차이가 있어요오. 오늘은 약간 달달한 쪽이 당기네요오~",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬 });
        SetOrder(3, "오늘은 컨디션이 좋아요오! 독한 칵테일 도전!",
            requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈, Define.ProofGrade.매우셈 });
        SetOrder(3, "알알이 모여있는게 정말 귀여운 과일이 있죠오. 뭔지 아시겠어요오?",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.석류 });
        SetOrder(3, "알싸함과 신 맛의 조합... 정말 기대되는 맛이지 않나요오?",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.생강, Define.CocktailTag.레몬 });
        #endregion
        #region 4단계 주문
        SetOrder(4, "좀 이상한 이름의 칵테일이 있다고 들었는데에... '꾸벅 이불에'였나아? 무슨 맛인지가 궁금해요오. ",
            requiredCocktail: new List<CocktailName>() { CocktailName.CubaLibre });
        SetOrder(4, "\"오렌지 불렀어?\" 라는 칵테일이 있다는데... 이름이 문장형이라니, 참 이상하지 않아요오?",
            requiredCocktail: new List<CocktailName>() { CocktailName.OrangeBlossom });
        SetOrder(4, "우리가 흔히들 커피콩이라고 부르는 것은 사실 콩이 아니라 씨앗이라는 사실 아시나요오? 참 신기하죠오!",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.커피 });
        SetOrder(4, "제가 마시고 싶은 칵테일이 있는데, 이름이 너무 길어서 까먹었어요오...",
            requiredCocktail: new List<CocktailName>() { CocktailName.FrozenMargarita });
        SetOrder(4, "식물의 향이 가득한 칵테일이 마시고 싶어요오!",
            requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.민트, Define.CocktailTag.과일, Define.CocktailTag.아몬드, Define.CocktailTag.생강 });
        #endregion

        SetReactionSctipt("와아, 제가 원하던 그 맛이었어요오!", "맛이 괜찮네요오!", "맛없어요오...");
    }
}

public class Order
{
    public List<CocktailName> requiredCocktail = null;
    public List<Define.ProofGrade> requiredProofGrade = null;
    public List<Define.CocktailTag> requiredTag = null;
    public List<Define.ProofGrade> avoidProofGrade = null;
    public List<Define.CocktailTag> avoidTag = null;
    public string orderContents = "";


    public string CustomerName { get; set; }
}