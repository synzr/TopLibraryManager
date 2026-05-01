using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class UnknownCommand : ICommand
{
    private readonly IConsoleUIService _consoleUIService;

    public UnknownCommand(IConsoleUIService consoleUIService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLines([
            "Неизвестная команда.",
            "\tИспользуйте команду 'помощь' для просмотра списка доступных команд."
        ]);
        return true;
    }
}