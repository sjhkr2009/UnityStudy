using System.Diagnostics;

public static class UrlHelper {
    public enum Target {
        Main,
        TransactionInfo,
        News,
        Analysis
    }

    static string ToUrl(this Target target) {
        return target switch {
            Target.Main => "main",
            Target.TransactionInfo => "frgn",
            Target.News => "news",
            Target.Analysis => "coinfo",
            _ => ""
        };
    }

    public static string GetUrl(int code, Target target = Target.Main) {
        string url = $"https://finance.naver.com/item/{target.ToUrl()}.naver?code={code:000000}";
        return url;
    }
}