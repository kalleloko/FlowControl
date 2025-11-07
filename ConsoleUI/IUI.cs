namespace FlowControl.Utilities;

public interface IUI
{
    public T AskForInput<T>(string? prompt = null, string? errorMessage = "Ogiltigt värde, försök igen!");
    //public T AskFor
    public T SelectInput<T>(Dictionary<char, T> options, string? prompt = null, string? errorMessage = "Ogiltigt val, försök igen!");
    public void PrintLine(string? prompt);
    public void PrintErrorLine(string? prompt);
}