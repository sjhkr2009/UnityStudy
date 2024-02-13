namespace Algorithm2024; 

public class Array06 : ISolutionExample {
    int MaxAmountOfWater(List<int> height) {
        int leftIndex = 0;
        int rightIndex = height.Count - 1;
        int maxAmount = 0;

        while (leftIndex < rightIndex) {
            int h = Math.Min(height[leftIndex], height[rightIndex]);
            int w = rightIndex - leftIndex;
            int amount = h * w;
            maxAmount = Math.Max(amount, maxAmount);

            if (height[leftIndex] < height[rightIndex]) leftIndex++;
            else rightIndex--;
        }

        return maxAmount;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MaxAmountOfWater(new List<int>() { 1, 8, 6, 2, 5, 4, 8, 3, 7 }));
        ConsoleUtility.WriteLine(MaxAmountOfWater(new List<int>() { 1, 1 }));
    }
}