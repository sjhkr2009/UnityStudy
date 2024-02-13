namespace Algorithm2024; 

public class Array01 : ISolutionExample {
    List<int> TwoSum(List<int> nums, int target) {
        Dictionary<int, int> requiredNumIndices = new Dictionary<int, int>();

        for (int i = 0; i < nums.Count; i++) {
            var current = nums[i];
            var required = target - current;

            if (requiredNumIndices.TryGetValue(current, out var idx)) {
                return new List<int>() { idx, i };
            }
            
            requiredNumIndices[required] = i;
        }

        return new List<int>();
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(TwoSum(new List<int>() {2, 7, 11, 15}, 9));
        ConsoleUtility.WriteLine(TwoSum(new List<int>() {3, 2, 4}, 6));
        ConsoleUtility.WriteLine(TwoSum(new List<int>() {3, 3}, 6));
    }
}