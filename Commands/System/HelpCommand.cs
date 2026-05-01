using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class HelpCommand : ICommand
{
    public string Name => "помощь";
    public IEnumerable<string> Aliases => new[] { "help" };
    public string Description => "Отображение справочного сообщения";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly CommandRegistry _commandRegistry;
    private readonly ICommandFactory _commandFactory;

    public HelpCommand(
        IConsoleUIService consoleUIService, 
        CommandRegistry commandRegistry,
        ICommandFactory commandFactory)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
        _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
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
            
            // Получаем описание команды из ее метаданных
            var description = GetCommandDescriptionFromMetadata(commandName);
            if (!string.IsNullOrEmpty(description))
            {
                _consoleUIService.WriteLine($"    {description}");
            }
        }
        
        return true;
    }

    private string GetCommandDescriptionFromMetadata(string commandName)
    {
        try
        {
            // Создаем экземпляр команды через фабрику для получения метаданных
            var command = _commandFactory.CreateCommand(commandName);
            if (command != null)
            {
                return command.Description;
            }
        }
        catch
        {
            // Если не удалось создать команду, возвращаем пустую строку
        }
        
        return string.Empty;
    }
}