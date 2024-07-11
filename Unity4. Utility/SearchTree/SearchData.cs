using System;

public class SearchData {
    /** 메인 키워드이며 한글일 경우 초성 검색을 지원함 */
    public string Word { get; }
    
    /** 이 단어를 검색하기 위한 추가적인 키워드들이며 초성 검색은 지원하지 않음 */
    public string[] ExtraWords { get; }

    public SearchData(string word, params string[] extraWords) {
        Word = word;
        ExtraWords = extraWords ?? Array.Empty<string>();
    }

    public override string ToString() {
        return $"Word: {Word}, Extra: {string.Join(", ", ExtraWords)}";
    }
}
