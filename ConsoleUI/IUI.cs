using System;

namespace FlowControl.Utilities;

public interface IUI
{
    public T AskForInput<T>(string? prompt = null, string? errorMessage = "Ogiltigt värde, försök igen!");
    //public T AskFor
    public string SelectInput(Dictionary<char, string> options, string? prompt = null, string? errorMessage = "Ogiltigt val, försök igen!");
    public T SelectInput<T>(Dictionary<char, T> options, Func<T, string> displayFunc, string? prompt = null, string ? errorMessage = "Ogiltigt val, försök igen!");
    public void PrintLine(string? prompt);
    public void PrintErrorLine(string? prompt);

    public void PrintEmptyLines(int lineCount = 1);
}