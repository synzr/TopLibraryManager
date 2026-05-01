using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.System;

public class HelloCommand : ICommand
{
    public string Name => "привет";
    public IEnumerable<string> Aliases => new[] { "hello" };
    public string Description => "Приветственное сообщение";
    
    private readonly IConsoleUIService _consoleUIService;

    public HelloCommand(IConsoleUIService consoleUIService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("Привет, мир!");
        return true;
    }
}