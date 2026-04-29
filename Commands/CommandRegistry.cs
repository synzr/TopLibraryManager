using System;
using System.Collections.Generic;
using System.Linq;

namespace TopLibraryManager.Commands;

/// <summary>
/// Реестр для сопоставления имен команд с экземплярами ICommand
/// </summary>
public class CommandRegistry
{
    private readonly Dictionary<string, ICommand> _commands = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _aliasesToPrimary = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _primaryCommands = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Регистрирует команду с ее основным именем
    /// </summary>
    /// <param name="name">Основное имя команды</param>
    /// <param name="command">Экземпляр команды</param>
    public void RegisterCommand(string name, ICommand command)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя команды не может быть пустым", nameof(name));

        _commands[name] = command ?? throw new ArgumentNullException(nameof(command));
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

        if (!_commands.TryGetValue(commandName, out var command))
            throw new ArgumentException($"Команда '{commandName}' не зарегистрирована", nameof(commandName));

        _commands[alias] = command;
        _aliasesToPrimary[alias] = commandName;
    }

    /// <summary>
    /// Получает команду по имени (или псевдониму)
    /// </summary>
    /// <param name="commandName">Имя команды или псевдоним</param>
    /// <returns>Экземпляр ICommand или null, если не найден</returns>
    public ICommand? GetCommand(string commandName)
    {
        _commands.TryGetValue(commandName, out var command);
        return command;
    }

    /// <summary>
    /// Проверяет, зарегистрирована ли команда
    /// </summary>
    /// <param name="commandName">Имя команды или псевдоним</param>
    /// <returns>True, если команда существует</returns>
    public bool HasCommand(string commandName)
    {
        return _commands.ContainsKey(commandName);
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
        return _commands.Keys.OrderBy(c => c);
    }
}