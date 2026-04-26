using Spectre.Console;
public class Program
{
    static Bot bot = new();

    static void Main()
    {
        string input = GetOptionInput();
        if(input == "Guess from answer")
        {
            bot.GuessFromAnswer(GetAnswerInput());
        } else
        {
            bot.StartGuessingFromUser();

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[grey]Provide the outcome in the form of:[/]");
            AnsiConsole.MarkupLine("[green]green as 2[/]");
            AnsiConsole.MarkupLine("[yellow]yellow as 1[/]");
            AnsiConsole.MarkupLine("[grey]gray as 0[/]");
            AnsiConsole.MarkupLine("[white]ex, 01201 \n[/]");

            AnsiConsole.MarkupLine("[white]The first guess was SALET[/]");
            string outcome = AnsiConsole.Prompt<string>(new TextPrompt<string>("What was the outcome?"));
            string guess = "salet";

            for(var i = 0; i < 5; i++)
            {
                guess = bot.GuessFromUser(outcome, guess);
                Console.WriteLine(guess);
                AnsiConsole.MarkupLine("[white]The next guess was " + guess.ToUpper() + "[/]");

                AnsiConsole.MarkupLine("Is it correct yet?");
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices("yes", "no")
                    );
                if(choice == "yes")
                {
                    break;
                }

                outcome = AnsiConsole.Prompt<string>(new TextPrompt<string>("What was the outcome?"));
            }
        }
        AnsiConsole.MarkupLine("[green]Thank you for using WordleBot[/]");
    }
    static string GetOptionInput()
    {
        AnsiConsole.MarkupLine("[green]Type of bot?[/]");
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Guess from answer", "Guess without knowing answer")
            );

        return choice;
    }
    static string GetAnswerInput()
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[green]What is the wordle answer?[/]");
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>("")
                .Validate(x =>
                    x.Length == 5 && bot.possibleWords.Contains(x)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Must a valid wordle word[/]"))
        );
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]The bot guessed: [/]");
        return input;
    }
}