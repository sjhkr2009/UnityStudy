namespace Algorithm2024; 

public class Array02 : ISolutionExample {
    int MaxProfit(List<int> prices) {
        int buyPrice = int.MaxValue;
        int maxProfit = int.MinValue;
        
        for (int i = 0; i < prices.Count; i++) {
            int currentPrice = prices[i];

            int profit = currentPrice - buyPrice;
            if (profit > maxProfit) {
                maxProfit = profit;
            }

            if (currentPrice < buyPrice) buyPrice = currentPrice;
        }

        return maxProfit.ClampMin(0);
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MaxProfit(new List<int>() { 7, 1, 5, 3, 6, 4 }));
        ConsoleUtility.WriteLine(MaxProfit(new List<int>() { 7, 6, 4, 3, 1 }));
    }
}