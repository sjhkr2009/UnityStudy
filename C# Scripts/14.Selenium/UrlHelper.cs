using System.Diagnostics;
using OpenQA.Selenium;

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

    public static void GoTo(this IWebDriver driver, int code) {
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        
        // 해당 Url로 이동한다.
        driver.Url = GetUrl(code);
        Console.WriteLine($"URL: {driver.Url}");
        
        // Url 로딩 후에는 페이지의 요소를 찾는 시간을 제거한다.
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
    }
}