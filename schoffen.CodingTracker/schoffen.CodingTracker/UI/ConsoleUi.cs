using System.Diagnostics;
using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Exceptions;
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
        AnsiConsole.WriteLine($"\n{session.StartTime} | {session.EndTime} | {session.GetFormattedDuration()}\n");
    }

    public bool TryShowSessionsTable(List<CodingSession> sessions)
    {
        if (sessions.Count == 0)
        {
            ShowMessage(UiMessage.NoSessionsFound);
            WaitForUser();
            return false;
        }
        
        const string format = "dd/MM/yyyy HH:mm:ss";
        var table = new Table();

        table.AddColumns("Start", "End", "Duration");

        foreach (var codingSession in sessions)
        {
            table.AddRow(codingSession.StartTime.ToString(format), codingSession.EndTime.ToString(format),
                codingSession.GetFormattedDuration());
        }

        AnsiConsole.Write(table);
        WaitForUser();
        return true;
    }

    public void ShowSelectedSessionMenu(CodingSession codingSession)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Markup(
                $"[bold cyan]Session {codingSession.StartTime} | {codingSession.EndTime} | {codingSession.GetFormattedDuration()}:[/]\n\n[cyan]Select an option:[/]\n")
        );
    }

    public void ShowMessage(UiMessage uiMessage)
    {
        var text = uiMessage switch
        {
            UiMessage.StartingNewSession => "Starting New Session",
            UiMessage.TrackingSession => "Tracking session...",
            UiMessage.PressEnterToStop => "Press ENTER to stop",
            UiMessage.NoSessionsFound => "No sessions found",
            UiMessage.OperationCanceled => "Operation Canceled",
            UiMessage.SessionUpdated => "Your coding session was updated",
            UiMessage.SessionDeleted => "Your coding session was deleted",
            UiMessage.OnlyOneSessionFound => "Only one session found. Selecting it automatically.",
            _ => throw new ArgumentOutOfRangeException(nameof(uiMessage), uiMessage, null)
        };
        
        AnsiConsole.MarkupLine(text);
        WaitForUser();
    }

    public void ShowExceptionMessage(CodingTrackerException exception)
    {
        AnsiConsole.MarkupLine(exception.Message);
        WaitForUser();
    }

    public void ShowElapsedTime(TimeSpan elapsed)
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        AnsiConsole.Write($@"Elapsed time: {elapsed:hh\:mm\:ss}");
    }

    public DateTime GetDateTimeInput(DateType dateType)
    {
        var label = dateType switch
        {
            DateType.Start => "start",
            DateType.End => "end",
            _ => throw new ArgumentOutOfRangeException(nameof(dateType))
        };

        var dateTimeInput = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter {label} date (format: dd/MM/yyyy HH:mm:ss):")
                .Validate(input => ValidationHelper.IsDateTimeValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid. Make sure to use this format: dd/MM/yyyy HH:mm:ss\nDate time: "))
        );

        return DateTime.Parse(dateTimeInput);
    }

    public DateTime GetDateInput()
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter date for reference (format: dd/MM/yyyy):")
                .Validate(input => ValidationHelper.IsDateValid(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid. Make sure to use this format: dd/MM/yyyy\nDate: "))
        );

        return DateTime.Parse(dateInput);
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

    public bool GetUserConfirmation(UiConfirmationMessages uiConfirmationMessages)
    {
        var text = uiConfirmationMessages switch
        {
            UiConfirmationMessages.ConfirmUpdate => "Are you sure you want to update?",
            UiConfirmationMessages.ConfirmDelete => "Are you sure you want to delete?",
            UiConfirmationMessages.ConfirmSelectSession => "Would you like to select a session?",
            _ => throw new ArgumentOutOfRangeException(nameof(uiConfirmationMessages), uiConfirmationMessages, null)
        };
        
        return AnsiConsole.Prompt(
            new TextPrompt<bool>(text)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));
    }

    public MainMenuOptions GetMainMenuOption() => PromptEnum<MainMenuOptions>();

    public MySessionsOptions GetMySessionsOption() => PromptEnum<MySessionsOptions>();

    public SortDirection GetSortDirectionOption() => PromptEnum<SortDirection>();

    public FilterPeriodOptions GetFilterPeriodOption() => PromptEnum<FilterPeriodOptions>();

    public SelectedSessionOptions GetSelectedSessionOption() => PromptEnum<SelectedSessionOptions>();

    public CodingSession SelectCodingSession(List<CodingSession> sessions)
    {
        if (sessions.Count != 1)
            return AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                    .AddChoices(sessions)
                    .UseConverter(session =>
                        $"{session.StartTime} | {session.EndTime} | {session.GetFormattedDuration()}"));
        
        ShowMessage(UiMessage.OnlyOneSessionFound);
        WaitForUser();
        return sessions[0];
    }

    private static TEnum PromptEnum<TEnum>() where TEnum : struct, Enum
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<TEnum>()
                .UseConverter(option => option.GetDescription())
                .AddChoices(Enum.GetValues<TEnum>())
        );
    }

    public void WaitForUser()
    {
        AnsiConsole.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
}