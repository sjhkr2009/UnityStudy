namespace Algorithm2024; 

public class Array07 : ISolutionExample {
    List<List<int>> ThreeSum(List<int> numbers) {
        var result = new List<List<int>>();

        List<int> nums = numbers;
        nums.Sort();

        for (int i = 0; i < nums.Count; i++) {
            if (i > 0 && nums[i - 1] == nums[i]) continue;

            int left = i + 1;
            int right = nums.Count - 1;

            while (left < right) {
                int sum = nums[i] + nums[left] + nums[right];
                if (sum == 0) {
                    result.Add(new List<int>() { nums[i], nums[left], nums[right] });

                    while (left < right && nums[left] == nums[left + 1]) left++;
                    while (left < right && nums[right] == nums[right - 1]) right--;

                    left++;
                    right--;
                }
                else if (sum < 0) {
                    left++;
                }
                else {
                    right--;
                }
            }
        }

        return result;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(ThreeSum(new List<int>() { -1, 0, 1, 2, -1, -4 }));
        ConsoleUtility.WriteLine(ThreeSum(new List<int>() { 0, 1, 1 }));
        ConsoleUtility.WriteLine(ThreeSum(new List<int>() { 0, 0, 0 }));
    }
}