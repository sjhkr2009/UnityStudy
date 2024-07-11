using System.Text;

/// <summary>
/// 한글 -> 초성 변환용 유틸리티
/// </summary>
public static class HangulUtils {
    private static readonly char[] Chosung = {
        'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ',
        'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
    };

    public static string GetChosung(string input) {
        var sb = new StringBuilder();
        foreach (char ch in input) {
            if (ch >= 0xAC00 && ch <= 0xD7A3) {
                int code = ch - 0xAC00;
                int chosungIndex = code / (21 * 28);
                sb.Append(Chosung[chosungIndex]);
            }
            else {
                sb.Append(ch);
            }
        }

        return sb.ToString();
    }
}
