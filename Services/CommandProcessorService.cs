using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;

namespace TopLibraryManager.Services;

public class CommandProcessorService(IConsoleUIService consoleUIService) : ICommandProcessorService
{
    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    private readonly IConsoleUIService _consoleUIService = consoleUIService;

    /// <inheritdoc />
    /// <exception cref="NotImplementedException"></exception>
    public bool ProcessCommand(CommandRequest commandRequest)
    {
        var command = commandRequest.Command.ToLower();

        switch (command)
        {
            // Команда приветствия
            case "привет":
                _consoleUIService.WriteLines(["Привет, мир!"]);
                break;

            // Комнада выхода из приложения
            case "выход":
                return false;

            // Обработка неизвестной команды
            default:
                _consoleUIService.WriteLines([
                    "Неизвестная команда.",
                    "\tПопробуйте ввести 'привет' или 'выход'."
                ]);
                break;
        }

        return true;
    }
}
