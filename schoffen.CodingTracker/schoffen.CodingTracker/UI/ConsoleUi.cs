using schoffen.CodingTracker.Extensions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.UI.Options;
using schoffen.CodingTracker.Validator;
using Spectre.Console;

namespace schoffen.CodingTracker.UI;

public class ConsoleUi : IUserInterface
{
    public void ShowMainMenu()
    {
        AnsiConsole.Write(
            new Markup("[bold cyan]Welcome to Coding Tracker[/]\n[gray]Select an option to begin[/]\n")
            );
    }

    public void ShowMySessionsMenu()
    {
        AnsiConsole.Write(
            new Markup("[bold cyan]My Sessions[/]\n[gray]Select an option[/]\n")
        );
    }

    public void ShowCodingSession(CodingSession session)
    {
        AnsiConsole.Write($"{session.StartTime} | {session.EndTime} | {session.GetFormattedDuration()}");
    }

    public void ShowSessionsTable(List<CodingSession> sessions)
    {
        var table = new Table();

        table.AddColumns("Start", "End", "Duration");

        foreach (var codingSession in sessions)
        {
            table.AddRow(codingSession.StartTime.ToLongTimeString(), codingSession.EndTime.ToLongTimeString(),
                codingSession.GetFormattedDuration());
        }

        AnsiConsole.Write(table);
    }

    public void ShowMessage(string message)
    {
        AnsiConsole.WriteLine(message);
    }

    public string GetDateTimeInput()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter datetime (format: dd/MM/yyyy HH:mm:ss)")
                .Validate(input => ValidationHelper.IsDateValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid date. Make sure to use this format: dd/MM/yyyy HH:mm:ss"))
        );
    }

    public bool GetUserConfirmation(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>(message)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));
    }

    public MainMenuOptions GetMainMenuOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<MainMenuOptions>())
        );
    }

    public MySessionsOptions GetMySessionsOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MySessionsOptions>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<MySessionsOptions>())
        );
    }

    public FilterOrderOptions GetFilterOrderOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<FilterOrderOptions>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<FilterOrderOptions>())
        );
    }

    public CodingSession SelectCodingSession(List<CodingSession> sessions)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
                .AddChoices(sessions)
                .UseConverter(session =>
                    $"{session.StartTime} | {session.EndTime} | {session.GetFormattedDuration()}"));
    }
}