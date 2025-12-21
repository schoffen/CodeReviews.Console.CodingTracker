using schoffen.CodingTracker.Extensions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.UI.Options;
using schoffen.CodingTracker.Validator;
using Spectre.Console;

namespace schoffen.CodingTracker.UI;

public class ConsoleUi : IUserInterface
{
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
                    {
                        var durationSpam = TimeSpan.FromSeconds(session.Duration);
                        return $@"{session.StartTime} | {session.EndTime} | {durationSpam:hh\:mm\:ss}";
                    }
            ));
    }

    public void ShowSessionsTable(List<CodingSession> sessions)
    {
        // TODO: finish this last class method
        throw new NotImplementedException();
    }

    public void ShowMessage(string message)
    {
        AnsiConsole.WriteLine(message);
    }
}