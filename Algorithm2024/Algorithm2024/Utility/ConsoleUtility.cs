using System.Collections;
using System.Text;

namespace Algorithm2024; 

public static class ConsoleUtility {
    public static void WriteLine() => WriteLine(null);
    public static void WriteLine(object? target) {
        if (target == null) {
            Console.WriteLine();
            return;
        }
        
        switch (target) {
            case IEnumerable<int> container:
                PrintInternal(container);
                break;
            case IEnumerable<IEnumerable<int>> dimensionalContainer:
                dimensionalContainer.ForEach(PrintInternal);
                Console.WriteLine("----------------");
                break;
            default:
                Console.WriteLine(target);
                break;
        }
    }
    
    private static void PrintInternal<T>(IEnumerable<T> container) {
        var log = new StringBuilder();
        log.Append('(');
        container.ForEach(e => log.Append($"{e},"));
        log.Remove(log.Length - 1, 1);
        log.Append(')');
        
        Console.WriteLine(log);
    }
}