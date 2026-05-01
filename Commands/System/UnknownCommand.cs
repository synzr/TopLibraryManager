using System;
using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class UnknownCommand : ICommand
{
    public string Name => "unknown";
    public IEnumerable<string> Aliases => Array.Empty<string>();
    public string Description => "Обработка неизвестных команд";
    
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