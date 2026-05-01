namespace TopLibraryManager.Commands.System;

public class ExitCommand : ICommand
{
    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        // Вывод не требуется, просто возвращаем false для сигнала выхода
        return false;
    }
}