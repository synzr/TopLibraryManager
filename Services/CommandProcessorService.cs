using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;
using TopLibraryManager.Commands;

namespace TopLibraryManager.Services;

public class CommandProcessorService(
    IConsoleUIService consoleUIService,
    ILibrarianService librarianService
) : ICommandProcessorService
{
    /// <summary>
    /// Реестр команд 
    /// </summary>
    private readonly CommandRegistry _commandRegistry = InitializeCommandRegistry(
        consoleUIService,
        librarianService
    );

    /// <inheritdoc />
    public bool ProcessCommand(CommandRequest commandRequest)
    {
        var commandName = commandRequest.Command.ToLower();
        
        // Получаем команду из реестра
        var command = _commandRegistry.GetCommand(commandName)
            ?? _commandRegistry.GetCommand("unknown");

        return command!.Execute(commandRequest.Args);
    }

    /// <summary>
    /// Инициализирует реестр команд со всеми доступными командами
    /// </summary>
    private static CommandRegistry InitializeCommandRegistry(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService)
    {
        var registry = new CommandRegistry();

        // регистрируем команды с их основными именами
        registry.RegisterCommand(
            "привет",
            new HelloCommand(consoleUIService, librarianService)
        );

        registry.RegisterCommand(
            "рег",
            new RegisterLibrarianCommand(consoleUIService, librarianService)
        );
        registry.RegisterAlias("регистрациябиблиотекаря", "рег");

        registry.RegisterCommand(
            "кто",
            new GetLibrarianCommand(consoleUIService, librarianService)
        );

        registry.RegisterCommand(
            "удлбиб",
            new DeleteLibrarianCommand(consoleUIService, librarianService)
        );
        registry.RegisterAlias("удалитьбиблиотекаря", "удлбиб");

        registry.RegisterCommand(
            "помощь",
            new HelpCommand(consoleUIService, librarianService, registry)
        );

        registry.RegisterCommand(
            "выход",
            new ExitCommand(consoleUIService, librarianService)
        );

        registry.RegisterCommand(
            "unknown",
            new UnknownCommand(consoleUIService, librarianService)
        );

        return registry;
    }
}
