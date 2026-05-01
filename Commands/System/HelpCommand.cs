using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class HelpCommand : ICommand
{
    private readonly IConsoleUIService _consoleUIService;
    private readonly CommandRegistry _commandRegistry;

    public HelpCommand(IConsoleUIService consoleUIService, CommandRegistry commandRegistry)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
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
            "новаякнига" => "Добавление новой книги в библиотеку",
            "изменитькнигу" => "Обновление информации о книге по ID",
            "удалитькнигу" => "Удаление книги по ID",
            "книги" => "Поиск книг по названию, автору, жанру или году",
            "новыйчитатель" => "Добавление нового читателя в библиотеку",
            "изменитьчитателя" => "Обновление информации о читателе по ID",
            "удалитьчитателя" => "Удаление читателя по ID",
            "читатели" => "Поиск читателей по ФИО, email или телефону",
            "посмотретьчитателя" => "Получение информации о читателе по ID",
            "новыйбиблиотекарь" => "Регистрация нового библиотекаря",
            "удалитьбиблиотекаря" => "Удаление библиотекаря по логину или ID",
            "посмотретьбиблиотекаря" => "Получение информации о библиотекаре по логину или ID",
            _ => string.Empty
        };
    }
}