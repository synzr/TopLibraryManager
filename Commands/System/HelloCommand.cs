using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class HelloCommand : ICommand
{
    private readonly IConsoleUIService _consoleUIService;

    public HelloCommand(IConsoleUIService consoleUIService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("Привет, мир!");
        return true;
    }
}