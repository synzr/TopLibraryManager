using System;
using System.Collections.Generic;

namespace TopLibraryManager.Commands;

public interface ICommand
{
    /// <summary>
    /// Основное имя команды
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Псевдонимы команды (альтернативные имена)
    /// </summary>
    IEnumerable<string> Aliases { get; }
    
    /// <summary>
    /// Описание команды для справки
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Выполняет команду с заданными аргументами
    /// </summary>
    /// <param name="args">Аргументы команды</param>
    /// <returns>True если приложение должно продолжить работу, false если должно завершиться</returns>
    bool Execute(string[] args);
}