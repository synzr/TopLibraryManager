using System;
using System.Collections.Generic;
using System.Linq;

namespace TopLibraryManager.Commands;

public class CommandRegistry
{
    private readonly ICommandFactory _commandFactory;
    private readonly Dictionary<string, string> _aliasesToPrimary = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _primaryCommands = new(StringComparer.OrdinalIgnoreCase);

    public CommandRegistry(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        
        var commandNames = _commandFactory.GetAvailableCommandNames();
        foreach (var name in commandNames)
        {
            _primaryCommands.Add(name);
        }

        foreach (var primaryName in _primaryCommands)
        {
            var aliases = _commandFactory.GetAliasesForCommand(primaryName);
            foreach (var alias in aliases)
            {
                _aliasesToPrimary[alias] = primaryName;
            }
        }
    }

    /// <summary>
    /// Регистрирует команду с ее основным именем
    /// </summary>
    /// <param name="name">Основное имя команды</param>
    /// <param name="command">Экземпляр команды (ignored in new architecture)</param>
    public void RegisterCommand(string name, ICommand command)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя команды не может быть пустым", nameof(name));

        _primaryCommands.Add(name);
    }

    /// <summary>
    /// Регистрирует псевдоним для уже зарегистрированной команды
    /// </summary>
    /// <param name="alias">Псевдоним команды</param>
    /// <param name="commandName">Основное имя команды, которое было ранее зарегистрировано</param>
    public void RegisterAlias(string alias, string commandName)
    {
        if (string.IsNullOrWhiteSpace(alias))
            throw new ArgumentException("Псевдоним не может быть пустым", nameof(alias));

        _aliasesToPrimary[alias] = commandName;
    }

    /// <summary>
    /// Получает команду по имени (или псевдониму)
    /// </summary>
    /// <param name="commandName">Имя команды или псевдоним</param>
    /// <returns>Экземпляр ICommand или null, если не найден</returns>
    public ICommand? GetCommand(string commandName)
    {
        if (string.IsNullOrWhiteSpace(commandName))
            return null;

        if (_aliasesToPrimary.TryGetValue(commandName, out var primaryName))
        {
            commandName = primaryName;
        }

        return _commandFactory.CreateCommand(commandName);
    }

    /// <summary>
    /// Проверяет, зарегистрирована ли команда
    /// </summary>
    /// <param name="commandName">Имя команды или псевдоним</param>
    /// <returns>True, если команда существует</returns>
    public bool HasCommand(string commandName)
    {
        if (string.IsNullOrWhiteSpace(commandName))
            return false;

        if (_primaryCommands.Contains(commandName))
            return true;

        return _aliasesToPrimary.ContainsKey(commandName);
    }

    /// <summary>
    /// Получает список всех основных команд (без псевдонимов)
    /// </summary>
    /// <returns>Коллекция имен основных команд</returns>
    public IEnumerable<string> GetPrimaryCommands()
    {
        return _primaryCommands.OrderBy(c => c);
    }

    /// <summary>
    /// Получает псевдонимы для указанной основной команды
    /// </summary>
    /// <param name="primaryCommandName">Основное имя команды</param>
    /// <returns>Коллекция псевдонимов команды</returns>
    public IEnumerable<string> GetAliasesForCommand(string primaryCommandName)
    {
        return _aliasesToPrimary
            .Where(kvp => kvp.Value.Equals(primaryCommandName, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Key)
            .OrderBy(a => a);
    }

    /// <summary>
    /// Получает все зарегистрированные имена команд (основные и псевдонимы)
    /// </summary>
    /// <returns>Коллекция всех имен команд</returns>
    public IEnumerable<string> GetAllCommandNames()
    {
        var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var primary in _primaryCommands)
        {
            allNames.Add(primary);
        }
        
        foreach (var alias in _aliasesToPrimary.Keys)
        {
            allNames.Add(alias);
        }
        
        return allNames.OrderBy(c => c);
    }
}