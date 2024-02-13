namespace Algorithm2024; 

public class Array05 : ISolutionExample {
    int SearchInRotatedArray(List<int> nums, int target) {
        int result = -1;
        
        SearchRecursive(0, nums.Count - 1);
        return result;
        
        void SearchRecursive(int startIdx, int endIdx) {
            if (nums[startIdx] == target) {
                result = startIdx;
                return;
            }
            if (nums[endIdx] == target) {
                result = endIdx;
                return;
            }
            if (endIdx - startIdx <= 1) return;
            
            int middleIdx = startIdx + (endIdx - startIdx) / 2;
            if (nums[startIdx] < nums[middleIdx]) {
                if (nums[startIdx] < target && target <= nums[middleIdx]) SearchRecursive(startIdx, middleIdx);
                else SearchRecursive(middleIdx + 1, endIdx);
            } else {
                if (nums[middleIdx] < target && target < nums[endIdx]) SearchRecursive(middleIdx + 1, endIdx);
                else SearchRecursive(startIdx, middleIdx);
            }
        }
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(SearchInRotatedArray(new List<int>() {4, 5, 6, 7, 0, 1, 2}, 0));
        ConsoleUtility.WriteLine(SearchInRotatedArray(new List<int>() {4, 5, 6, 7, 0, 1, 2}, 3));
        ConsoleUtility.WriteLine(SearchInRotatedArray(new List<int>() {1}, 0));
    }
}