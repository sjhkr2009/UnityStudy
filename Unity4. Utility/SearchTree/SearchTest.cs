using System.Collections.Generic;
using UnityEngine;

public class SearchTest : MonoBehaviour {
    List<SearchData> SearchDatas = new List<SearchData> {
        new SearchData("검은 칼", "전설의 검"),
        new SearchData("하얀 방패", "모든 공격을 막아줌"),
        new SearchData("허연 방패", "적당한 공격을 막아줌"),
        new SearchData("하얀 검", "개쎔", "방패고 뭐고 다깸"),
        new SearchData("붉은 검", "화염의 힘이 깃든 검"),
        new SearchData("파란 물약", "마나를 회복시켜줌"),
        new SearchData("초록 포션", "독을 해독해줌"),
        new SearchData("검은 망토", "어둠 속에서 숨을 수 있음"),
        new SearchData("강철 갑옷", "강력한 방어력을 제공함"),
        new SearchData("금빛 투구", "황금으로 만들어진 투구"),
        new SearchData("은빛 방패", "은으로 만든 방패"),
        new SearchData("불타는 활", "화염 화살을 발사함"),
        new SearchData("얼음 창", "적을 얼릴 수 있는 창"),
        new SearchData("번개 검", "전기의 힘을 가진 검"),
        new SearchData("신속의 신발", "이동 속도를 증가시킴"),
        new SearchData("힘의 반지", "착용자의 힘을 증가시킴"),
        // ...
    };

    private SearchTree SearchTree { get; } = new SearchTree();

    private void Start() {
        foreach (var data in SearchDatas) {
            SearchTree.Insert(data);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Semicolon)) {
            Test();
        }
    }

    private void Test() {
        var keywords = new List<string> { "하얀", "ㅎㅇ", "방패", "ㅂㅍ", "ㅇ ㅂ", "얀 방", "증가", "ㅅㅅ", "검" };
        
        foreach (var keyword in keywords) {
            var searchResults = SearchTree.Search(keyword);
            Debug.Log($"Keyword: {keyword}");
            foreach (var result in searchResults) {
                Debug.Log(result);
            }
        }
        
        Debug.Log($"Data Count : {SearchTree.DataCount}");
    }
}
