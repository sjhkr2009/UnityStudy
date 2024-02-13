namespace Algorithm2024; 

public class DynamicProgramming02 : ISolutionExample {
    int UniquePaths(int m, int n) {
        var map = new int[m, n];
        map[0, 0] = 1;

        for (int x = 0; x < m; x++) {
            for (int y = 0; y < n; y++) {
                int cur = map[x, y];
                if (x + 1 < m) map[x + 1, y] += cur;
                if (y + 1 < n) map[x, y + 1] += cur;
            }
        }

        return map[m - 1, n - 1];
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(UniquePaths(3, 2));
        ConsoleUtility.WriteLine(UniquePaths(3, 7));
    }
}