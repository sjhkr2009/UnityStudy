namespace Algorithm2024; 

public class Array10 : ISolutionExample {
    int MaxProductOfSubArray(List<int> nums) {
        int min = nums[0];
        int max = nums[0];
        int result = nums[0];
        CheckSubArrayRecursive(0, nums.Count - 1);

        return result;

        void CheckSubArrayRecursive(int startIdx, int endIdx) {
            if (startIdx == endIdx) return;

            CheckSubArrayRecursive(startIdx, endIdx - 1);
            int last = nums[endIdx];

            if (last < 0) {
                (min, max) = (max, min);
            }

            min = Math.Min(last, min * last);
            max = Math.Max(last, max * last);
            result = Math.Max(max, result);
        }
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MaxProductOfSubArray(new List<int>() { 2, 3, -2, 4 }));
        ConsoleUtility.WriteLine(MaxProductOfSubArray(new List<int>() { -2, 0, -1 }));
        ConsoleUtility.WriteLine(MaxProductOfSubArray(new List<int>() { 2, 3, -2, 4, -1, 0, -6, 4, 5, 10, -2 }));
    }
}