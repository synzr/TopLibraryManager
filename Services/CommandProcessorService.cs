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
    IBookService bookService,
    IReaderService readerService
) : ICommandProcessorService
{
    /// <summary>
    /// Реестр команд
    /// </summary>
    private readonly CommandRegistry _commandRegistry = InitializeCommandRegistry(
        consoleUIService,
        librarianService,
        bookService,
        readerService
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
        IBookService bookService,
        IReaderService readerService)
    {
        var registry = new CommandRegistry();

        // регистрируем команды с их основными именами
        registry.RegisterCommand(
            "привет",
            new HelloCommand(consoleUIService, librarianService, bookService, readerService)
        );

        registry.RegisterCommand(
            "рег",
            new RegisterLibrarianCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("регистрациябиблиотекаря", "рег");

        registry.RegisterCommand(
            "кто",
            new GetLibrarianCommand(consoleUIService, librarianService, bookService, readerService)
        );

        registry.RegisterCommand(
            "удлбиб",
            new DeleteLibrarianCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("удалитьбиблиотекаря", "удлбиб");

        registry.RegisterCommand(
            "помощь",
            new HelpCommand(consoleUIService, librarianService, bookService, readerService, registry)
        );

        registry.RegisterCommand(
            "выход",
            new ExitCommand(consoleUIService, librarianService, bookService, readerService)
        );

        registry.RegisterCommand(
            "unknown",
            new UnknownCommand(consoleUIService, librarianService, bookService, readerService)
        );

        // регистрируем команды для работы с книгами
        registry.RegisterCommand(
            "добавитькнигу",
            new CreateBookCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("новаякнига", "добавитькнигу");
        registry.RegisterAlias("добкн", "добавитькнигу");

        registry.RegisterCommand(
            "обновитькнигу",
            new UpdateBookCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("изменитькнигу", "обновитькнигу");
        registry.RegisterAlias("обнкн", "обновитькнигу");

        registry.RegisterCommand(
            "удалитькнигу",
            new DeleteBookCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("удлкн", "удалитькнигу");

        registry.RegisterCommand(
            "найтикниги",
            new SearchBooksCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("поисккниг", "найтикниги");
        registry.RegisterAlias("книги", "найтикниги");
        registry.RegisterAlias("найкн", "найтикниги");

        // регистрируем команды для работы с читателями
        registry.RegisterCommand(
            "добавитьчитателя",
            new CreateReaderCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("новыйчитатель", "добавитьчитателя");
        registry.RegisterAlias("добчт", "добавитьчитателя");

        registry.RegisterCommand(
            "обновитьчитателя",
            new UpdateReaderCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("изменитьчитателя", "обновитьчитателя");
        registry.RegisterAlias("обнчт", "обновитьчитателя");

        registry.RegisterCommand(
            "удалитьчитателя",
            new DeleteReaderCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("удлчт", "удалитьчитателя");

        registry.RegisterCommand(
            "найтичитателей",
            new SearchReadersCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("поискчитателей", "найтичитателей");
        registry.RegisterAlias("читатели", "найтичитателей");
        registry.RegisterAlias("найчт", "найтичитателей");

        registry.RegisterCommand(
            "получитьчитателя",
            new GetReaderCommand(consoleUIService, librarianService, bookService, readerService)
        );
        registry.RegisterAlias("чт", "получитьчитателя");
        registry.RegisterAlias("инфочт", "получитьчитателя");

        return registry;
    }
}
