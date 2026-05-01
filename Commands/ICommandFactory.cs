using System;

namespace TopLibraryManager.Commands;

public interface ICommandFactory
{
    /// <summary>
    /// Создает экземпляр команды по ее имени
    /// </summary>
    /// <param name="commandName">Имя команды (без учета регистра)</param>
    /// <returns>Экземпляр команды или null, если не найдена</returns>
    ICommand? CreateCommand(string commandName);
    
    /// <summary>
    /// Получает все доступные имена команд
    /// </summary>
    /// <returns>Коллекция имен команд</returns>
    IEnumerable<string> GetAvailableCommandNames();

    /// <summary>
    /// Получает все псевдонимы для указанного основного имени команды
    /// </summary>
    /// <param name="primaryCommandName">Основное имя команды</param>
    /// <returns>Коллекция псевдонимов</returns>
    IEnumerable<string> GetAliasesForCommand(string primaryCommandName);
}