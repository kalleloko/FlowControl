using FlowControl.Utilities;

namespace FlowControl;

internal class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        FlowControlTask task = new FlowControlTask(ui);
        task.initUILoop();
    }
}
