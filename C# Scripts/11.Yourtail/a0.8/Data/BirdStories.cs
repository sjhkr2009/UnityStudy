using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

//[CreateAssetMenu(fileName = "Scripts", menuName = "CustomDatabase/Texts", order = int.MinValue + 1)]
public class BirdStories
{
    [ShowInInspector, ReadOnly] public bool StorySettingDone { get; private set; }
    [ShowInInspector, ReadOnly] public Dictionary<int, List<string>> eagleStory { get; private set; } = new Dictionary<int, List<string>>();
    [ShowInInspector, ReadOnly] public Dictionary<int, List<string>> parrotStory { get; private set; } = new Dictionary<int, List<string>>();
    [ShowInInspector, ReadOnly] public Dictionary<int, List<string>> flamingoStory { get; private set; } = new Dictionary<int, List<string>>();
    [ShowInInspector, ReadOnly] public Dictionary<int, List<string>> swanStory { get; private set; } = new Dictionary<int, List<string>>();
    [ShowInInspector, ReadOnly] public Dictionary<int, List<string>> penguinStory { get; private set; } = new Dictionary<int, List<string>>();


    public List<string> GetDialog(Customers customer, int storyIndex = -1)
    {
        if (storyIndex <= 0) storyIndex = customer.Level - 1;
        List<string> result = null;

        switch (customer.CustomerType)
        {
            case Define.CustomerType.Eagle:
                return eagleStory.TryGetValue(storyIndex, out result) ? result : null;
            case Define.CustomerType.Parrot:
                return parrotStory.TryGetValue(storyIndex, out result) ? result : null;
            case Define.CustomerType.Flamingo:
                return flamingoStory.TryGetValue(storyIndex, out result) ? result : null;
            case Define.CustomerType.Swan:
                return swanStory.TryGetValue(storyIndex, out result) ? result : null;
            case Define.CustomerType.Penguin:
                return penguinStory.TryGetValue(storyIndex, out result) ? result : null;
            default:
                Debug.Log($"{customer.Name} 캐릭터의 {storyIndex} 번째 스토리 텍스트가 없습니다.");
                return null;
        }
    }

    [Button]
    public void DialogSetting()
    {
        SetEagelStory();
        SetParrotStory();
        SetFlamingoStory();
        SetSwanStory();
        SetPenguinStory();

        StorySettingDone = true;
    }

    void SetEagelStory()
    {
        if (eagleStory.Count > 0)
            eagleStory.Clear();

        for (int i = 1; i <= Define.BirdMaxLevel[(int)Define.CustomerType.Eagle]; i++)
            eagleStory.Add(i, null);

        eagleStory[1] = new List<string>()
        {
            "허허~ 분위기 좋구만~!! 여기 쐬주 한잔에,,,,",
            "읎,,다고~? 요즘 메뉴는,, 알아먹기도 힘들구만!!"
        };
        eagleStory[2] = new List<string>()
        {
            "걱정이 있어 뵌다고,,,? 거참..",
            "실은 손주 녀석이 요즘 통 연락이 없구만.",
            "어릴 적에는 말도 참 잘 듣고 씩씩했는데, 요즘은 매일 한숨만 쉬지 뭐냐?",
            "젊은 나이에 열정도 의지도 없고 말이야"
        };
        eagleStory[3] = new List<string>()
        {
            "얼마 전에 손주랑 좀 다투긴 했어.",
            "회사 면접에서 또 뭐가 잘 안됐다는 구만.",
            "요즘 계속 힘들다고만 해대던데 그렇게 매사에 부정적이면 어른들도 안 좋게 볼 거 아니냐.",
            "어려운 게 있어도 칠전팔기의 정신으로 열정을 보여줘야 인정받는다... 이 말이야.",
            "그래서 내가 한 마디 했더니 화를 내면서 끊어버리지 뭐냐? 요즘 애들은 예의도 참..."
        };
        eagleStory[4] = new List<string>()
        {
            "그려,, 한참 힘들 때이기는 해.",
            "그래도 젊다는 게 뭐냐? 내가 그 나이 때도 힘든 적 많았다.",
            "그때에 비하면 지금 우리나라가 살기 얼마나 좋은데.",
            "뭐 그래도 크게 걱정은 안 한다.",
            "원체 열심히 하던 아이니까 조만간 일어서겠지."
        };
        eagleStory[5] = new List<string>()
        {
            "설날에 어멈이랑 아범만 온다는 구나. 손주 녀석은 아직도 상심해 있으려나...",
            "...",
            "요즘 애들은 원. 뭐가 그리도 잘났는지... 나 때는 못 보던 것들이 매일같이 나오는데 어찌어찌 해내는 걸 보면 신기하단 말이야.",
            "그러니 우리 손주도 힘들겠지. 그 잘난 애들 사이에서 뭔가 보여주고 싶어하니…",
            "..."
        };
        eagleStory[6] = new List<string>()
        {
            "흐흠, 흠. 좋은 일 있냐고? 늙은이가 딱히 그런 게 있겠나. 그냥 뭐…",
            "어제 손주한테 연락이 왔다네. 미안하다는 말이 나오지 않아 안부를 물었는데, 자기가 먼저 미안하다지 뭐냐.",
            "뭔지 모를 외국말을 섞어서 막 얘기하는데, 자기 딴에는 열심히 이것저것 해 보려는데 제대로 안 풀리는가 보더라.",
            "나 때는 말이다…",
            "내가 어릴 적에도 어른들은 그런 말을 했었지. 옷이 그게 뭐냐, 예의가 없다, 왜 인생을 허비하느냐... 나는 그 말을 자식들한테 똑같이 했지만 자식들은 말을 듣지 않더군",
            "그래도 아범이나 어멈이 지금 잘 살고 있는 걸 보면, 나름대로 열심히 했던 모양이야.",
            "그렇다고 내가 틀렸다는 건 아닐세. 난 한평생을 성실하게 살아왔다고 자부할 수 있다네. 어려웠지만 이를 악물고 노력했고 인정받는 동료이자 가장이었네.",
            "...",
            "하지만 내 자식들도 그랬겠지. 내 말은 지지리도 안 들었지만 열심히 잘 살고 있는 듯하니. 자식한테 한 실수를 손주한테 또 한 것 같구먼.",
            "...",
            "다음엔 요즘 애들 먹는 술도 먹어봐야겠어."
        };
    }
    void SetParrotStory()
    {
        if (parrotStory.Count > 0)
            parrotStory.Clear();

        for (int i = 1; i <= Define.BirdMaxLevel[(int)Define.CustomerType.Parrot]; i++)
            parrotStory.Add(i, null);

        parrotStory[1] = new List<string>()
        {
            "매력적인 건 피곤한 일이에요.",
            "이런 데서나 할 수 있는 얘기죠."
        };
        parrotStory[2] = new List<string>()
        {
            "어떤 걸 보여드릴까요? 화려한 깃털, 아니면 능수능란한 화술?",
            "아쉽게도 오늘은 휴무일이네요. 좀 쉬고 싶거든요.",
            "정 그런 것들이 궁금하다면 동물원으로 찾아가는 건 어때요?",

        };
        parrotStory[3] = new List<string>()
        {
            "……옛날엔 동물원에서 일했어요. 다들 나를 아주 좋아했죠. 예쁘다, 화려하다, 귀엽다, 사랑스럽다. 좋은 말들을 참 많이 들었어요.",
            "나를 보기 위해서 찾아오는 사람들도 많았으니 감사할 일이죠.",
            "분명 감사한 일인데…… 모르겠어요.",
            "그냥 이젠 쉬고 싶어요.",

        };
        parrotStory[4] = new List<string>()
        {
            "그럴 때가 있지 않나요? 갑자기 모든 게 잘못된 것처럼 느껴질 때. 나한테는 그 날이 꼭 그랬어요. 별다른 날도 아니었어요. 그저 항상 있던 일이었죠.",
            "그날 나는 하루 종일 지쳐 있었어요. 잠을 청하려던 참이었죠. 막 잠에 들려던 참에, 한 어린아이가 철창에 얼굴을 가까이 대고는 큰 소리로 외쳤어요. ",
            "“우와, 너 되게 예쁘다! 말도 잘 한다며? 이것도 따라해 봐!” 그리고는 계속 큰 소리로 뭐라고 이야기를 하는데, 세상이 빙글빙글 도는 것만 같았어요.",
            "그날 밥은 평소보다 맛이 없었죠. 그리고 갑자기 모든 게 바뀌었어요."
        };
        parrotStory[5] = new List<string>()
        {
            "그 다음 날, 동물원 일을 그만뒀어요. 더이상 거기에 있을 수가 없었거든요.",
            "뭐 그런 걸로 새삼스레 힘들어 하냐고 다들 이야기했지만, 나는 더이상 견딜 수가 없었어요.",
            "사람들의 시선을 끄는 게 평생의 자랑거리였는데. 그걸 잃고 나니 이 세상에 내가 할 수 있는 게 없는 것만 같았어요. ",
            "무기력했죠...한참을 방황했어요. 처음 이 바를 찾았을 때도 그맘때였던 것 같아요. 정말 이상한 건, 아무런 해결책도 생기지 않았지만 점점 마음이 편해졌다는 거예요.",
            "당신이 만들어 준 따뜻한 칵테일도 한 몫 했을 것 같네요."
        };
        parrotStory[6] = new List<string>()
        {
            "여기가 참 좋아요. 이 곳의 음료는 마음을 편안하게 해 주는 것 같아요.",
            "얼마 전부터는 글을 조금씩 쓰기 시작했어요. 쉬는 동안 느꼈던 것들을 하나씩 써 내려가는 중이에요. 물론 이 곳에 대한 이야기도 있어요.",
            "많은 사람들에게 사랑받을수록 더 행복한 삶이라고 생각해 왔는데, 꼭 그렇지도 않나 봐요.",
            "사람들 사이에서 정신없이 살던 때보다 지금이 더 즐겁거든요. 책이 나오면 꼭 제일 먼저 보여 줄게요.",
            "……늘 고마워요."
        };
    }
    void SetFlamingoStory()
    {
        if (flamingoStory.Count > 0)
            flamingoStory.Clear();

        for (int i = 1; i <= Define.BirdMaxLevel[(int)Define.CustomerType.Flamingo]; i++)
            flamingoStory.Add(i, null);

        flamingoStory[1] = new List<string>()
        {
            "우리는 무리 지어 생활하는데 나는 우리 무리의 대장이야. 어떻게 대장이 되었냐고?",
            "우리는 다 멋지지만 그 중에 단연 내가 최고라고 자부할 수 있지ㅎㅎ",
            "어떻게 아냐고? 이 깃 좀 봐. 너무 찬란하지 않니? 이렇게 윤기나는 깃털은 내가 제일이야. 나는 핑크색을 너무 사랑해.",
            "너도 핑크가 좋지? 그렇지?"
        };
        flamingoStory[2] = new List<string>()
        {
            "오늘 뭔 일이 있었는 줄 아니?",
            "숲속 파티에 갔는데 거기서 내가 올해의 새 대회에서 1등을 했지 뭐야. 뭐 이미 예상했어~ 나를 넘어설 새는 없지 없지~^^",
            "내년에도 후년에도 내가 당연 1등 아니겠어?ㅎㅎ",
            "재수 없다고? 부러우면 지는거랬어!",
        };
        flamingoStory[3] = new List<string>()
        {
            "나는 요새 고민이 많아.",
            "항상 빡빡한 일정에 많은 친구과 놀면서도 어딘가 모르게 공허한 느낌이 들어.",
            "누구에게 보여지는 나를 신경 쓰게 되고 점점 내 삶을 사는게 아닌 것 같다고 느꼈어. 항상 멋지고 아름답고 완벽한 나지만 그런 나를 유지하기 위해 너네 들은 내가 얼마나 무수한 노력을 하는지 모를 거야.",
            "이제 슬슬 웃고 있는 입꼬리에 경련이 오기 시작했네ㅎㅎ",
            "그치만 말야... 바쁜 일상에서 벗어나서 가끔 혼자 고독의 시간을 즐기고 싶어.",
            "하지만 그럼 외롭겠지? 너도 나와 비슷한 생각을 할 때가 있니?"
        };
        flamingoStory[4] = new List<string>()
        {
            "안녕 친구! 오랜만이야! ㅎㅎ 잘 지냈지?",
            "조언대로 혼자만의 여행을 떠나고 숲속 마을로 돌아왔어^^",
            "확실히 나만의 시간을 보내고 나니 마음도 한결 가볍고 산뜻해졌어~! 다 너의 덕분인 거 같아. 고마워^^",
            "돌아올 때 다들 어떻게 알았는지 친구들이 잊지 않고 나를 마중 나와줬어. 마음이 뭉클했어. 사실 눈물도 찔끔 흘렸지 뭐야..부끄럽게!ㅠ",
            "나는 이제 나만의 자유로운 삶을 살기로 결정했어. 겉 모습에 신경 쓰는 걸 줄이고 내면을 가꿀 거야.",
            "물론 원래도 내면이 아름답긴 했어! 소중한 친구들이 대변해 준다고~",
            "겉 모습에 신경 좀 안 써도 나는 아름다우니깐ㅎㅎ 당연히 너도!! 우리 모두!!"
        };
    }
    void SetSwanStory()
    {
        if (swanStory.Count > 0)
            swanStory.Clear();

        for (int i = 1; i <= Define.BirdMaxLevel[(int)Define.CustomerType.Swan]; i++)
            swanStory.Add(i, null);

        swanStory[1] = new List<string>()
        {
            "나의 과거이야기를 들어줄래? 나는 어릴 때 범상치 않은 오리였어.",
            "어느 날 저 언덕에 올라가 내리막길을 향해 빠르게 달리며 힘차게 날개를 뻗었지.",
            "잠깐이나마 맡은 윗공기는 생각했던 것보다 상쾌하고 기분 좋더라고.",
            "비록 얼마 안돼서 땅으로 떨어졌지만 말이야. 그래도 너무 인상 깊은 추억이였어!"
        };
        swanStory[2] = new List<string>()
        {
            "그러다가 한 오리친구를 만났는데 그 친구가 자기 할아버지는 날 수 있었다고 하는 거야.",
            "나는 당연히 그 말을 믿을 수 없었지. 그래도 속으로 내심 기분이 좋았어.",
            "그래, 우리는 사실 날 수 있던 거였어. 나도 열심히 노력하면 언젠가 하늘을 날 수 있지 않을까?"
        };
        swanStory[3] = new List<string>()
        {
            "형제들이 요새 나를 자꾸 괴롭혀. 다른 오리들과 다르게 나는 못생겼다나?",
            "그런 걸로 나를 슬프게 할 순 없을 걸? 내가 다른 오리들과는 다르게 특별하다는 거 아니겠어?",
            "나는 남들과는 다르다고~ 누구나 다 같을 필요는 없잖아?"
        };
        swanStory[4] = new List<string>()
        {
            "나는 실험하는 걸 좋아해. 뒷 동산에 올라가 내 날개에 작은 프로펠러 모터를 달았지!",
            "결과가 궁금해?",
            "당연히 성공적이었지! 저번에 낙하산을 메고 날았을 때보다 훨씬 더 오랫동안 날았다고!",
            " 하지만 나는 내 혼자 힘으로 날고 싶어. 그래서 매일 나는 연습을 꾸준히 하고 있어. 곧 이 기분을 매일 느낄 수 있을 거야."
        };
        swanStory[5] = new List<string>()
        {
            "나는 우연히 날개를 푸드덕거렸어.",
            "그런데 갑자기 날기 시작한 거야. 이럴 수가!",
            "옆에 있던 호수의 백조 아주머니가 나에게 “백조 아가야 무슨 일 있니?”라고 말했어. 그래 나는 백조였다는 사실을 이제야 알게 되었어. 그래서 형제들과 다른 모습이였던거야!",
            "내 말이 맞았어. 나는 특별하다니까? 백조 아주머니가 나에게 백조 친구들을 소개시켜줬어. 친구들과 함께 날아다니며 즐거운 하루를 보냈어. 정말 꿈 같은 하루였지!",
            "나는 오늘을 절대 잊지 못할거야. 나의 이야기는 마지막이지만 우리는 끝이 아니야.",
            "나는 항상 호수근처에 있을 테니 내가 보고싶을 땐 언제나 호수로 놀러 와~ 환영이야!"
        };
    }
    void SetPenguinStory()
    {
        if (penguinStory.Count > 0)
            penguinStory.Clear();

        for (int i = 1; i <= Define.BirdMaxLevel[(int)Define.CustomerType.Penguin]; i++)
            penguinStory.Add(i, null);

        penguinStory[1] = new List<string>()
        {
            "바텐더 씨는 여기서 일한지 오래 되셨나요오?",
            "…바텐더 씨도 초보시군요오.",
            "아, 그냥 궁금해서 물어보았어요오. 칵테일이 맛있네요오."
        };
        penguinStory[2] = new List<string>()
        {
            "바텐더 씨는 친구 많은 편이세요오? 왠지 인기쟁이일 것 같은데에.",
            "…",
            "막 이래~ 재밌자고 한 말이에요오.",
            "별거 아니고 제가 친해지고 싶은 분이 있는데에, 저는 소심한 편이라 다가가는게 힘들어요오.",
            "말씀하시는 것들을 들으면 되게 어른스러우시고, 멋지다는 생각이 드는 분인데요오.",
            "어떻게 하면 친해질 수 있을까 고민이 되네요오.",
            "저는 이 타지에 아는 새도 한 명 없고 그래서 고민이에요오.",
            "...",
            "바텐더 씨가 친구가 되어주시겠다고요오?",
            "저야 싫지는 않지만 친구가 “친구하자!” 하고 딱 되는건 아니잖아요오.",
            "...",
            "그러네요오. 저도 바보 같은 짓을 했던 것 같네요오.",
            "고마워요오. 이제 어떻게 해야할지 알 것 같아요오."
        };
        penguinStory[3] = new List<string>()
        {
            "바텐더 씨, 전 가끔씩 걱정이 막 생길 때가 있어요오.",
            "내가 지금 잘 하고 있는걸까아? 내가 너무 게으르게 살고 있는건 아닐까아?",
            "그런 생각을 하다보면 너무 슬퍼져요오. 나는 잘 해내야만 하는데에…",
            "이럴 때면 세상에 나 혼자밖에 없는 기분이 들기도 하고오…",
            "에고고, 또 막 떠들어댔네요오. 칵테일 맛있었어요오. 나중에 또 얘기해요오."
        };
        penguinStory[4] = new List<string>()
        {
            "바텐더 씨, 저는 있잖아요오. 어렸을 때부터 식물학자가 되고 싶었어요오.",
            "꽃과 나무들은 저희에게 생명의 신비함과 아름다움을 알려주는 정말 멋있는 존재들이라고 저는 생각해요오.",
            "문제는 제가 살고 있던 남극은 식물 공부을 하기에는 매우 힘든 곳이고오…",
            "그래서 식물 공부를 하고, 꿈을 이루기 위해서 이 먼 곳까지 왔어요오.",
            "이 곳에 온 지는 몇 개월 안되긴 했지만, 벌써 가족들이 그리워요오…"
        };
        penguinStory[5] = new List<string>()
        {
            "바텐더 씨. 제가 예전에 친해지고 싶은 새가 있다고 말씀드렸었잖아요오?",
            "그 분하고 오늘 이야기도 되게 오래 하고 좀 친해진 것 같아요오.",
            "알고 보니 그 분도 머얼리서 오셨더라구요오. 저처럼 하고 싶은 일이 있었대요오.",
            "그리고 지금의 자리에 있기까지 수많은 고난들을 겪으면서, 이겨낼 수 있도록 노력하셨더라구요오.",
            "그 분과 대화를 나누면서 여러 가지 느낀게 있었어요오.",
            "저는 여태까지 제가 머나먼 고향에서 떠나온 것을 힘들다고 생각했고, 그래서 여기저기 제 힘든점을 털어놓곤 했어요오.",
            "그러면서 그 과정에서, 그 힘든 고난과 문제들을 어쩌면 회피하려고 했던걸지도 몰라요오.",
            "근데 그 분은 달랐어요오. 본인에게 주어진 역경들을 바로 마주하고, 해결하려고 노력했어요오.",
            "...",
            "누구나 각자 인생에서 힘든 점은 하나씩 갖고 사는 것 같아요.",
            "저는 너무 여기저기 찡찡대고 불평불만만 했던 것 같은데, 사실 그런걸로 문제가 해결되지는 않죠. 힘든 일이 있을 땐 불평불만하지 않고, 그 문제를 해결하도록 마주하고 노력해야해요.",
            "물론 그 과정에서 힘든 일이 있으면 다른 사람들에게 속내를 털어놓을 수는 있겠죠. 그게 나쁜 건 아니에요.",
            "하지만 본인의 문제를 해결할 수 있는 사람은 결국 본인 스스로 밖에 없어요.",
            "...",
            "그냥 그렇다구요오. 칵테일이 맛있네요."
        };
    }
}