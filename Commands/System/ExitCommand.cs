using System.Collections.Generic;

namespace TopLibraryManager.Commands.System;

public class ExitCommand : ICommand
{
    public string Name => "выход";
    public IEnumerable<string> Aliases => new[] { "exit", "quit" };
    public string Description => "Завершение работы приложения";
    
    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        // Вывод не требуется, просто возвращаем false для сигнала выхода
        return false;
    }
}