namespace Algorithm2024; 

public class Array08 : ISolutionExample {
    List<int> ProductExceptSelf(List<int> nums) {
        var zeroIndices = new HashSet<int>();
        int multipleAll = 1;

        for (int i = 0; i < nums.Count; i++) {
            int cur = nums[i];
            if (cur == 0) {
                zeroIndices.Add(i);
                continue;
            }

            multipleAll *= cur;
        }

        var result = new List<int>();
        int zeroCount = zeroIndices.Count;

        for (int i = 0; i < nums.Count; i++) {
            // (0 개수 == 0) -> (전체 곱 / 해당 숫자)
            // (0 개수 = 1) -> 그거 빼고 0
            // (0 개수 >= 2) -> 전부 0
            
            if (zeroCount == 0)
                result.Add(multipleAll / nums[i]);
            else if (zeroCount == 1)
                result.Add(zeroIndices.Contains(i) ? multipleAll : 0);
            else
                result.Add(0);
        }

        return result;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(ProductExceptSelf(new List<int>() { 1, 2, 3, 4 }));
        ConsoleUtility.WriteLine(ProductExceptSelf(new List<int>() { -1, 1, 0, -3, 3 }));
    }
}