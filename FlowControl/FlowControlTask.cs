using FlowControl.Utilities;

namespace FlowControl;

internal class FlowControlTask
{
    private readonly IUI ui;

    private readonly Dictionary<char, KeyValuePair<string, Action>> menu;

    private const int stringRepeaterCount = 10;

    public FlowControlTask(IUI ui)
    {
        this.ui = ui;
        menu = new Dictionary<char, KeyValuePair<string, Action>>(){
            { '1', new ("Biobiljetter", GetCinemaPrice) },
            { '2', new ("Repetera text", RepeatText) },
            { '3', new ("Det tredje ordet", ThirdWord) },
            { '0', new ("Avsluta programmet", ExitProgram) }
        };
    }

    internal void initUILoop()
    {
        initUILoop(this.menu);
    }

    internal void initUILoop(Dictionary<char, KeyValuePair<string, Action>> menu)
    {
        while (true)
        {
            KeyValuePair<string, Action> choice = ui.SelectInput(
                menu,
                (kvp) => kvp.Key,
                ui.FormatSquare("Huvudmenu")
            );
            ui.PrintLine("─────────────────────────────────");
            ui.PrintEmptyLines();
            choice.Value.Invoke();
            ui.PrintEmptyLines();
            ui.PrintLine("─────────────────────────────────"); 
            ui.PrintEmptyLines();
        }
    }

    private void GetCinemaPrice()
    {
        int count = ui.AskForInput<int>("Hur många är ni?", "Svara med en siffra, tack!");
        if (count < 1)
        {
            ui.PrintErrorLine("Minst en person måste vara med!");
            GetCinemaPrice();
            return;
        }
        if (count > 30)
        {
            ui.PrintErrorLine("Max 30 personer är tillåtna!");
            GetCinemaPrice();
            return;
        }
        int totalPrice = 0;
        for (int i = 1; i <= count; i++)
        {
            int age = ui.AskForInput<int>(count > 1 ? $"Person {i}, ange ålder:" : "Ange ålder:");
            int price;
            string pricePrefix;
            switch (age)
            {
                case < 5:
                    price = 0;
                    pricePrefix = "Barn";
                    break;
                case < 20:
                    price = 80;
                    pricePrefix = "Ungdomspris";
                    break;
                case >= 100:
                    price = 0;
                    pricePrefix = "Hundra +";
                    break;
                case > 64:
                    price = 90;
                    pricePrefix = "Pensionärspris";
                    break;
                default:
                    price = 120;
                    pricePrefix = "Standardpris";
                    break;
            }
            ui.PrintLine($"{pricePrefix}: {price}kr");
            totalPrice += price;
        }
        if (count > 1)
        {
            ui.PrintLine($"Totalt pris för {count} personer: {totalPrice}kr");
        }
    }
    private void RepeatText()
    {
        string input = ui.AskForInput<string>($"Skriv något om du vill se det {stringRepeaterCount} gånger");
        for (int i = 0; i < stringRepeaterCount; i++)
        {
            ui.Print($"{i + 1}. {input}, ");
        }
        ui.PrintEmptyLines();
    }
    private void ThirdWord()
    {
        string? thirdWord = null;
        while (true)
        {
            string input = ui.AskForInput<string>("Skriv en mening med minst tre ord:");
            thirdWord = extractThirdWord(input);
            if(thirdWord != null)
            {
                break;
            }
            ui.PrintErrorLine("Meningen måste innehålla minst tre ord. Försök igen.");
        }
        ui.PrintLine($"Det tredje ordet är: {thirdWord}");
    }

    private string? extractThirdWord(string input)
    {
        string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length < 3)
        {
            return null;
        }
        string thirdWord = words[2];
        return thirdWord;
    }
    private void ExitProgram()
    {
        ui.PrintLine("Avslutar...");
        System.Environment.Exit(0);
    }

}