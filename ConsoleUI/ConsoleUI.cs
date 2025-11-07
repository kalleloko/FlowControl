using System.Reflection;

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
}
