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
    ILibrarianService librarianService,
    IBookService bookService
) : ICommandProcessorService
{
    /// <summary>
    /// Реестр команд
    /// </summary>
    private readonly CommandRegistry _commandRegistry = InitializeCommandRegistry(
        consoleUIService,
        librarianService,
        bookService
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
        ILibrarianService librarianService,
        IBookService bookService)
    {
        var registry = new CommandRegistry();

        // регистрируем команды с их основными именами
        registry.RegisterCommand(
            "привет",
            new HelloCommand(consoleUIService, librarianService, bookService)
        );

        registry.RegisterCommand(
            "рег",
            new RegisterLibrarianCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("регистрациябиблиотекаря", "рег");

        registry.RegisterCommand(
            "кто",
            new GetLibrarianCommand(consoleUIService, librarianService, bookService)
        );

        registry.RegisterCommand(
            "удлбиб",
            new DeleteLibrarianCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("удалитьбиблиотекаря", "удлбиб");

        registry.RegisterCommand(
            "помощь",
            new HelpCommand(consoleUIService, librarianService, bookService, registry)
        );

        registry.RegisterCommand(
            "выход",
            new ExitCommand(consoleUIService, librarianService, bookService)
        );

        registry.RegisterCommand(
            "unknown",
            new UnknownCommand(consoleUIService, librarianService, bookService)
        );

        // регистрируем команды для работы с книгами
        registry.RegisterCommand(
            "добавитькнигу",
            new CreateBookCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("новаякнига", "добавитькнигу");
        registry.RegisterAlias("добкн", "добавитькнигу");

        registry.RegisterCommand(
            "обновитькнигу",
            new UpdateBookCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("изменитькнигу", "обновитькнигу");
        registry.RegisterAlias("обнкн", "обновитькнигу");

        registry.RegisterCommand(
            "удалитькнигу",
            new DeleteBookCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("удлкн", "удалитькнигу");

        registry.RegisterCommand(
            "найтикниги",
            new SearchBooksCommand(consoleUIService, librarianService, bookService)
        );
        registry.RegisterAlias("поисккниг", "найтикниги");
        registry.RegisterAlias("книги", "найтикниги");
        registry.RegisterAlias("найкн", "найтикниги");

        return registry;
    }
}
