using System.Reflection;
using System.Text;

namespace FlowControl.Utilities;

public class ConsoleUI : IUI
{
    /// <summary>
    /// Ask the user for input of type T. Uses T.TryParse to validate input.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prompt">The question or instruction, if any</param>
    /// <param name="errorMessage">Message to display on parsing error</param>
    /// <returns></returns>
    public T AskForInput<T>(string? prompt, string? errorMessage)
    {
        PrintLine(prompt);

        string? input = Console.ReadLine();

        try
        {
            return ParseValue<T>(input);
        }
        catch (NotSupportedException ex)
        {
            PrintErrorLine(ex.Message);
            return default;
        }
        catch (Exception)
        {
            PrintErrorLine(errorMessage);
            return AskForInput<T>(prompt, errorMessage);
        }
    }

    public string SelectInput(Dictionary<char, string> options, string? prompt = null, string? errorMessage = "Ogiltigt val, försök igen!")
    {
        return SelectInput(options, (v) => v,  prompt, errorMessage);
    }
    public T SelectInput<T>(Dictionary<char, T> options, Func<T, string> displayFunc, string? prompt = null, string ? errorMessage = "Ogiltigt val, försök igen!")
    {   
        PrintLine(prompt);
        foreach (KeyValuePair<char, T> kvp in options)
        {
            string display = displayFunc(kvp.Value);
            PrintLine($"{kvp.Key}: {display}");
        }
        char selected = AskForInput<char>("Välj ett alternativ: ", errorMessage);
        if (options.ContainsKey(selected))
        {
            return options[selected];
        }
        else
        {
            PrintErrorLine(errorMessage);
            return SelectInput<T>(options, displayFunc, prompt, errorMessage);
        }
    }

    public void Print(string? prompt)
    {
        Console.Write(prompt);
    }

    /// <summary>
    /// Displays the specified prompt message to the console if it is not null, empty, or whitespace.
    /// </summary>
    /// <param name="prompt">The message to display. If null, empty, or whitespace, no output is produced.</param>
    public void PrintLine(string? prompt)
    {
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            Console.WriteLine(prompt);
        }
    }

    /// <summary>
    /// Displays the specified prompt message to the console if it is not null, empty, or whitespace.
    /// </summary>
    /// <param name="prompt">The message to display. If null, empty, or whitespace, no output is produced.</param>
    public void PrintErrorLine(string? prompt)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        PrintLine(prompt);
        Console.ResetColor();
    }

    /// <summary>
    /// Try to parse the input string to type T using T.TryParse.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown when T doesn't have TryParse method</exception>
    /// <exception cref="FormatException">Thrown when input could not be parsed into T</exception>
    private static T ParseValue<T>(string? input)
    {
        // special case for strings
        if (typeof(T) == typeof(string))
        {
            return (T)(object)(input ?? "");
        }

        // get TryParse(string, out T) method
        var tryParseMethod = typeof(T).GetMethod(
            "TryParse",
            BindingFlags.Public | BindingFlags.Static,
            null,
            new Type[] { typeof(string), typeof(T).MakeByRefType() },
            null
        );

        if (tryParseMethod == null)
        {
            // type T does not have a TryParse method
            throw new NotSupportedException(
                $"Type {typeof(T).Name} does not support TryParse."
            );
        }

        // create arguments for TryParse
        object?[] args = new object?[] { input, null };

        // call method: bool success = T.TryParse(input, out value)
        bool success = (bool)tryParseMethod.Invoke(null, args)!;
        if (success)
        {
            return (T)args[1]!; // args[1] holds the out result
        }
        throw new FormatException($"Could not parse '{input}' to {typeof(T).Name}.");
    }

    public void PrintEmptyLines(int lineCount = 1)
    {
        for(int i = 0; i < lineCount; i ++)
        {
            Console.WriteLine();
        }
    }

    public string FormatSquare(string content)
    {
        StringBuilder sb = new StringBuilder();
        int length = content.Length + 6;
        string emptyString = new string(' ', content.Length);
        sb.AppendLine("┌" + (new string('─', length)) + "┐");
        sb.AppendLine($"│   {emptyString}   │");
        sb.AppendLine($"│   {content}   │");
        sb.AppendLine($"│   {emptyString}   │");
        sb.AppendLine("└" + (new string('─', length)) + "┘");
        return sb.ToString();
    }
}
