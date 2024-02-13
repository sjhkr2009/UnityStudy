namespace Algorithm2024; 

public class Array04 : ISolutionExample {
    bool CheckDuplicate(List<int> nums) {
        var checkedNums = new HashSet<int>();

        return nums.Any(num => !checkedNums.Add(num));
    }
    
    public void PrintExample() {
        ConsoleUtility.WriteLine(CheckDuplicate(new List<int>() {1, 2, 3, 1}));
        ConsoleUtility.WriteLine(CheckDuplicate(new List<int>() {1, 2, 3, 4}));
        ConsoleUtility.WriteLine(CheckDuplicate(new List<int>() {1, 1, 1, 3, 3, 4, 3, 2, 4, 2}));
    }
}