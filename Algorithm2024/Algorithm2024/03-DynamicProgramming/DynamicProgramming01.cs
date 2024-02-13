namespace Algorithm2024; 

public class DynamicProgramming01 : ISolutionExample {
    int PossibleWaysToClimb(int n) {
        var ways = new List<int>(n) { 1, 2 };
        for (int i = 2; i < n; i++) {
            ways.Add(ways[i - 2] + ways[i - 1]);
        }

        return ways[n - 1];
    }
    
    public void PrintExample() {
        ConsoleUtility.WriteLine(PossibleWaysToClimb(2));
        ConsoleUtility.WriteLine(PossibleWaysToClimb(3));
    }
}