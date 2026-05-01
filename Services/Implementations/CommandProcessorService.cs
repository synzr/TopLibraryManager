using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;
using TopLibraryManager.Commands;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Services.Implementations;

public class CommandProcessorService : ICommandProcessorService
{
    /// <summary>
    /// Реестр команд
    /// </summary>
    private readonly CommandRegistry _commandRegistry;

    public CommandProcessorService(CommandRegistry commandRegistry)
    {
        _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
    }

    /// <inheritdoc />
    public bool ProcessCommand(CommandRequest commandRequest)
    {
        var commandName = commandRequest.Command.ToLower();
        
        // Получаем команду из реестра
        var command = _commandRegistry.GetCommand(commandName)
            ?? _commandRegistry.GetCommand("unknown");

        return command!.Execute(commandRequest.Args);
    }
}
