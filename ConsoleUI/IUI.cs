namespace FlowControl.Utilities;

public interface IUI
{
    public T AskForInput<T>(string prompt = "", string errorMessage = "");
}