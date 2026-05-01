using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TopLibraryManager.Commands.Books;
using TopLibraryManager.Commands.Librarians;
using TopLibraryManager.Commands.Readers;
using TopLibraryManager.Commands.System;

namespace TopLibraryManager.Commands;

/// <summary>
/// Реализация фабрики, создающей экземпляры команд с использованием dependency injection
/// </summary>
public class CommandFactory : ICommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _commandTypes;
    private readonly Dictionary<string, string> _aliases;

    public CommandFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _commandTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        _aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        DiscoverAndRegisterCommands();
    }

    /// <inheritdoc />
    public ICommand? CreateCommand(string commandName)
    {
        if (string.IsNullOrWhiteSpace(commandName))
            return null;

        if (_aliases.TryGetValue(commandName, out var primaryCommandName))
            commandName = primaryCommandName;

        if (!_commandTypes.TryGetValue(commandName, out var commandType))
            return null;

        try
        {
            return (ICommand)_serviceProvider.GetRequiredService(commandType);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create command '{commandName}'. Make sure it's registered in DI container.", ex);
        }
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAvailableCommandNames()
    {
        return _commandTypes.Keys.OrderBy(k => k);
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAliasesForCommand(string primaryCommandName)
    {
        if (string.IsNullOrWhiteSpace(primaryCommandName))
            return Enumerable.Empty<string>();

        return _aliases
            .Where(kvp => kvp.Value.Equals(primaryCommandName, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Key)
            .OrderBy(a => a);
    }

    /// <summary>
    /// Регистрирует команду с ее основным именем
    /// </summary>
    public void RegisterCommand(string name, Type commandType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Command name cannot be empty", nameof(name));
        
        if (commandType == null)
            throw new ArgumentNullException(nameof(commandType));
        
        if (!typeof(ICommand).IsAssignableFrom(commandType))
            throw new ArgumentException($"Type must implement {nameof(ICommand)}", nameof(commandType));

        _commandTypes[name] = commandType;
    }

    /// <summary>
    /// Регистрирует псевдоним для существующей команды
    /// </summary>
    public void RegisterAlias(string commandName, string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
            throw new ArgumentException("Alias cannot be empty", nameof(alias));
        
        if (string.IsNullOrWhiteSpace(commandName))
            throw new ArgumentException("Command name cannot be empty", nameof(commandName));
        
        if (!_commandTypes.ContainsKey(commandName))
            throw new ArgumentException($"Command '{commandName}' is not registered", nameof(commandName));

        _aliases[alias] = commandName;
    }

    /// <summary>
    /// Обнаруживает все реализации ICommand в сборке и регистрирует их
    /// </summary>
    private void DiscoverAndRegisterCommands()
    {
        RegisterCommand("новаякнига", typeof(CreateBookCommand));
        RegisterAliasIfExists("новаякнига", "добавитькнигу");
        RegisterAliasIfExists("новаякнига", "addbook");
        
        RegisterCommand("удалитькнигу", typeof(DeleteBookCommand));
        RegisterAliasIfExists("удалитькнигу", "deletebook");

        RegisterCommand("изменитькнигу", typeof(UpdateBookCommand));
        RegisterAliasIfExists("изменитькнигу", "обновитькнигу");
        RegisterAliasIfExists("изменитькнигу", "updatebook");

        RegisterCommand("книги", typeof(SearchBooksCommand));
        RegisterAliasIfExists("книги", "поисккниг");
        RegisterAliasIfExists("книги", "searchbooks");

        RegisterCommand("новыйбиблиотекарь", typeof(RegisterLibrarianCommand));
        RegisterAliasIfExists("новыйбиблиотекарь", "регистрациябиблиотекаря");
        RegisterAliasIfExists("новыйбиблиотекарь", "registerlibrarian");

        RegisterCommand("удалитьбиблиотекаря", typeof(DeleteLibrarianCommand));
        RegisterAliasIfExists("удалитьбиблиотекаря", "deletelibrarian");

        RegisterCommand("посмотретьбиблиотекаря", typeof(GetLibrarianCommand));
        RegisterAliasIfExists("посмотретьбиблиотекаря", "getlibrarian");

        RegisterCommand("новыйчитатель", typeof(CreateReaderCommand));
        RegisterAliasIfExists("новыйчитатель", "регистрациячитателя");
        RegisterAliasIfExists("новыйчитатель", "registerreader");

        RegisterCommand("удалитьчитателя", typeof(DeleteReaderCommand));
        RegisterAliasIfExists("удалитьчитателя", "deletereader");

        RegisterCommand("посмотретьчитателя", typeof(GetReaderCommand));
        RegisterAliasIfExists("посмотретьчитателя", "getreader");

        RegisterCommand("читатели", typeof(SearchReadersCommand));
        RegisterAliasIfExists("читатели", "поискчитателей");
        RegisterAliasIfExists("читатели", "searchreaders");

        RegisterCommand("изменитьчитателя", typeof(UpdateReaderCommand));
        RegisterAliasIfExists("изменитьчитателя", "обновитьчитателя");
        RegisterAliasIfExists("изменитьчитателя", "updatereader");

        RegisterCommand("выход", typeof(ExitCommand));
        RegisterAliasIfExists("выход", "exit");
        RegisterAliasIfExists("выход", "quit");

        RegisterCommand("привет", typeof(HelloCommand));
        RegisterAliasIfExists("привет", "hello");

        RegisterCommand("помощь", typeof(HelpCommand));
        RegisterAliasIfExists("помощь", "help");

        RegisterCommand("unknown", typeof(UnknownCommand));
    }

    /// <summary>
    /// Получает имя команды по умолчанию из имени типа
    /// </summary>
    private static string GetDefaultCommandName(Type commandType)
    {
        var typeName = commandType.Name;
        
        if (typeName.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
        {
            typeName = typeName.Substring(0, typeName.Length - "Command".Length);
        }
        
        return typeName.ToLowerInvariant();
    }

    private void RegisterAliasIfExists(string commandName, string alias)
    {
        if (_commandTypes.ContainsKey(commandName))
        {
            RegisterAlias(commandName, alias);
        }
    }
}