using System.Diagnostics;
using schoffen.CodingTracker.Enums;
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
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Markup("[bold cyan]Welcome to Coding Tracker[/]\n\n[cyan]Select an option:[/]\n")
            );
    }

    public void ShowMySessionsMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Markup("[bold cyan]My Sessions[/]\n\n[cyan]Select an option:[/]\n")
        );
    }

    public void ShowCodingSession(CodingSession session)
    {
        AnsiConsole.Write($"{session.StartTime} | {session.EndTime} | {session.GetFormattedDuration()}\n");
    }

    public void ShowSessionsTable(List<CodingSession> sessions)
    {
        const string format = "dd/MM/yyyy HH:mm:ss";
        var table = new Table();

        table.AddColumns("Start", "End", "Duration");

        foreach (var codingSession in sessions)
        {
            table.AddRow(codingSession.StartTime.ToString(format), codingSession.EndTime.ToString(format),
                codingSession.GetFormattedDuration());
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("\nPress any key to return");
        Console.ReadKey();
    }

    public void ShowMessage(string message)
    {
        AnsiConsole.WriteLine(message);
    }

    public string GetDateTimeInput(DateType dateType)
    {
        var label  = dateType switch
        {
            DateType.Start => "start",
            DateType.End => "end",
            _ => throw new ArgumentOutOfRangeException(nameof(dateType))
        };
        
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter {label } (format: dd/MM/yyyy HH:mm:ss):")
                .Validate(input => ValidationHelper.IsDateTimeValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid. Make sure to use this format: dd/MM/yyyy HH:mm:ss\nDate time: "))
        );
    }

    public string GetDateInput()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter date for reference (format: dd/MM/yyyy):")
                .Validate(input => ValidationHelper.IsDateValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid. Make sure to use this format: dd/MM/yyyy HH:mm:ss\nDate: "))
        );
    }

    public int GetYearInput()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter year for reference: ")
                .Validate(input => ValidationHelper.IsYearValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid. Make sure you only type number and not a future year"))
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

    public SortDirection GetSortDirectionOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<SortDirection>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<SortDirection>())
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

    public FilterPeriodOptions GetFilterPeriodOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<FilterPeriodOptions>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<FilterPeriodOptions>())
        );
    }
}