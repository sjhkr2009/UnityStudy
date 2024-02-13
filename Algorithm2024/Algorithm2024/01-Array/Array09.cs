namespace Algorithm2024; 

public class Array09 : ISolutionExample {
    int MinInRotateArray(List<int> nums) {
        int leftIndex = 0;
        int rightIndex = nums.Count - 1;

        while (leftIndex < rightIndex) {
            int midIndex = (leftIndex + rightIndex) / 2;

            if (nums[midIndex] > nums[rightIndex]) {
                leftIndex = midIndex + 1;
            } else {
                rightIndex = midIndex - 1;
            }
        }

        return nums[leftIndex];
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MinInRotateArray(new List<int>() { 3, 4, 5, 1, 2 }));
        ConsoleUtility.WriteLine(MinInRotateArray(new List<int>() { 4, 5, 6, 7, 0, 1, 2 }));
        ConsoleUtility.WriteLine(MinInRotateArray(new List<int>() { 11, 13, 15, 17 }));
    }
}