using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class UnknownCommand : BaseCommand
{
    public UnknownCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService) 
        : base(consoleUIService, librarianService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLines([
            "Неизвестная команда.",
            "\tИспользуйте команду 'помощь' для просмотра списка доступных команд."
        ]);
        return true;
    }
}