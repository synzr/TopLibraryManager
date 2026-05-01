using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class HelpCommand : BaseCommand
{
    /// <summary>
    /// Реестр команд
    /// </summary>
    private readonly CommandRegistry _commandRegistry;

    public HelpCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService,
        CommandRegistry commandRegistry)
        : base(consoleUIService, librarianService, bookService)
    {
        _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Доступные команды ===");
        
        var primaryCommands = _commandRegistry.GetPrimaryCommands();
        
        foreach (var commandName in primaryCommands)
        {
            // Пропускаем служебные команды "unknown" и "help" в основном списке
            if (commandName.Equals("unknown", StringComparison.OrdinalIgnoreCase))
                continue;
                
            var aliases = _commandRegistry.GetAliasesForCommand(commandName).ToList();
            
            if (aliases.Any())
            {
                _consoleUIService.WriteLine($"  {commandName} (также: {string.Join(", ", aliases)})");
            }
            else
            {
                _consoleUIService.WriteLine($"  {commandName}");
            }
            
            // Добавляем описание команды
            var description = GetCommandDescription(commandName);
            if (!string.IsNullOrEmpty(description))
            {
                _consoleUIService.WriteLine($"    {description}");
            }
        }
        
        _consoleUIService.WriteLine("\nДля получения подробной информации используйте: <команда> --help");
        return true;
    }

    private string GetCommandDescription(string commandName)
    {
        return commandName.ToLower() switch
        {
            "привет" => "Приветственное сообщение",
            "рег" => "Регистрация нового библиотекаря",
            "кто" => "Получение информации о библиотекаре по логину или ID",
            "удлбиб" => "Удаление библиотекаря по логину или ID",
            "выход" => "Завершение работы приложения",
            "помощь" => "Отображение этого справочного сообщения",
            "добавитькнигу" => "Добавление новой книги в библиотеку",
            "обновитькнигу" => "Обновление информации о книге по ID",
            "удалитькнигу" => "Удаление книги по ID",
            "найтикниги" => "Поиск книг по названию, автору, жанру или году",
            _ => string.Empty
        };
    }
}