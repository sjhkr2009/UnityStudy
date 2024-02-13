namespace Algorithm2024; 

public class Array03 : ISolutionExample {
    int MaxSubArray(List<int> nums) {
        int sum = nums[0];
        int maxSum = sum;
        
        for (int i = 1; i < nums.Count; i++) {
            int cur = nums[i];
            if (sum < 0) sum = 0;

            sum += cur;
            if (maxSum < sum) maxSum = sum;
        }

        return maxSum;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MaxSubArray(new List<int>() { -2, 1, -3, 4, -1, 2, 1, -5, 4 }));
        ConsoleUtility.WriteLine(MaxSubArray(new List<int>() { 1 }));
        ConsoleUtility.WriteLine(MaxSubArray(new List<int>() { 5, 4, -1, 7, 8 }));
    }
}