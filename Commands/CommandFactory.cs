using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TopLibraryManager.Commands.Books;
using TopLibraryManager.Commands.Librarians;
using TopLibraryManager.Commands.Loans;
using TopLibraryManager.Commands.Readers;
using TopLibraryManager.Commands.System;

namespace TopLibraryManager.Commands;

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
        // Список всех типов команд для регистрации
        var commandTypes = new List<Type>
        {
            // Books commands
            typeof(CreateBookCommand),
            typeof(DeleteBookCommand),
            typeof(UpdateBookCommand),
            typeof(SearchBooksCommand),
            
            // Librarians commands
            typeof(RegisterLibrarianCommand),
            typeof(DeleteLibrarianCommand),
            typeof(GetLibrarianCommand),
            
            // Readers commands
            typeof(CreateReaderCommand),
            typeof(DeleteReaderCommand),
            typeof(GetReaderCommand),
            typeof(SearchReadersCommand),
            typeof(UpdateReaderCommand),
            
            // Loans commands
            typeof(CreateLoanCommand),
            typeof(ReturnBookCommand),
            typeof(ListActiveLoansCommand),
            typeof(ViewLoanDetailsCommand),
            typeof(PayFineCommand),
            
            // System commands
            typeof(ExitCommand),
            typeof(HelloCommand),
            typeof(HelpCommand),
            typeof(UnknownCommand)
        };

        foreach (var commandType in commandTypes)
        {
            try
            {
                // Для HelpCommand используем специальную обработку из-за циклической зависимости
                if (commandType == typeof(HelpCommand))
                {
                    RegisterHelpCommandWithoutCircularDependency(commandType);
                    continue;
                }
                
                // Создаем экземпляр команды через DI для получения метаданных
                var command = (ICommand)_serviceProvider.GetRequiredService(commandType);
                
                // Регистрируем команду с ее основным именем
                var primaryName = command.Name;
                if (string.IsNullOrWhiteSpace(primaryName))
                {
                    // Fallback: используем имя типа по умолчанию
                    primaryName = GetDefaultCommandName(commandType);
                }
                
                RegisterCommand(primaryName, commandType);
                
                // Регистрируем все алиасы
                foreach (var alias in command.Aliases)
                {
                    if (!string.IsNullOrWhiteSpace(alias))
                    {
                        RegisterAlias(primaryName, alias);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но продолжаем регистрацию других команд
                Console.WriteLine($"Warning: Failed to register command {commandType.Name}: {ex.Message}");
            }
        }
    }
    
    private void RegisterHelpCommandWithoutCircularDependency(Type helpCommandType)
    {
        // Для HelpCommand используем хардкодированные метаданные, чтобы избежать циклической зависимости
        // HelpCommand зависит от ICommandFactory, который еще создается
        const string primaryName = "помощь";
        var aliases = new[] { "help" };
        
        RegisterCommand(primaryName, helpCommandType);
        
        foreach (var alias in aliases)
        {
            if (!string.IsNullOrWhiteSpace(alias))
            {
                RegisterAlias(primaryName, alias);
            }
        }
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