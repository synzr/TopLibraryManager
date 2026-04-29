using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;

namespace TopLibraryManager.Services;

public class ConsoleUIService : IConsoleUIService
{
    public ConsoleUIService()
    {
        // устанавливаем кодировку консоли в UTF-16
        Console.InputEncoding = Encoding.GetEncoding("utf-16");
        Console.OutputEncoding = Encoding.GetEncoding("utf-16");
    }

    /// <inheritdoc />
    public CommandRequest? ReadCommandRequest()
    {
        // читаем запрос из консоли
        Console.Write("> ");

        var rawRequest = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(rawRequest) || rawRequest == string.Empty)
        {
            return null;
        }

        // разбиваем запрос на команду и аргументы
        var command = rawRequest
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .First();
        var args = rawRequest
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();
        
        return new CommandRequest(command, args);
    }

    /// <inheritdoc />
    public void WriteLines(string[] lines)
    {
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }
}
