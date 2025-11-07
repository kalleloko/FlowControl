namespace FlowControl.Utilities;

public interface IUI
{
    public T AskForInput<T>(string? prompt = null, string? errorMessage = "Ogiltigt värde, försök igen!");
}