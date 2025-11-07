using FlowControl.Utilities;

namespace FlowControl;

internal class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        int testint = ui.AskForInput<int>("Skriv ett int: ", "Nej nej");
        Console.WriteLine(testint);
        char testchar = ui.AskForInput<char>("Skriv ett char: ");
        Console.WriteLine(testchar);
        float testfloat = ui.AskForInput<float>("Skriv ett float: ");
        Console.WriteLine(testfloat);
        long testlong = ui.AskForInput<long>("Skriv ett long: ");
        Console.WriteLine(testlong);
        DateTime testDateTime = ui.AskForInput<DateTime>("Skriv ett DateTime: ");
        Console.WriteLine(testDateTime);
    }
}
