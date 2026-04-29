using System;

namespace TopLibraryManager.Commands;

public interface ICommand
{
    /// <summary>
    /// Выполняет команду с заданными аргументами
    /// </summary>
    /// <param name="args">Аргументы команды</param>
    /// <returns>True если приложение должно продолжить работу, false если должно завершиться</returns>
    bool Execute(string[] args);
}