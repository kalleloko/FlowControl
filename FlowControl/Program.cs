using FlowControl.Utilities;

namespace FlowControl;

internal class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        int test = ui.AskForInput<int>("Skriv ett tecken: ");
        Console.WriteLine(test);
    }
}
