using FlowControl.Utilities;

namespace FlowControl;

internal class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        int ans = ui.SelectInput<int>(new Dictionary<char, int>()
        {
            {'a', 1 },
            {'b', 2 },
            {'c', 3 },
        }, "Välj en siffra:");
        Console.WriteLine(ans);
    }
}
