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
    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    /// <inheritdoc />
    public void WriteLines(string[] lines)
    {
        foreach (string line in lines)
        {
            WriteLine(line);
        }
    }

    /// <inheritdoc />
    public string? ReadLine(string? prompt)
    {
        // пишем промпт для ввода cnhjrb
        if (string.IsNullOrEmpty(prompt))
        {
            prompt = "> ";
        }
        Console.Write(prompt);

        // читаем строку из консоли и удаляем лишние пробелы
        var line = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }
        else
        {
            return line;
        }
    }

    /// <inheritdoc />
    public string? ReadPassword(string? prompt)
    {
        // пишем промпт для ввода пароля
        if (string.IsNullOrEmpty(prompt))
        {
            prompt = "> ";
        }
        Console.Write(prompt);

        // читаем пароль из консоли, маскируя ввод символами *
        var password = "";
        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        Console.WriteLine();

        // если пароль пустой, возвращаем null
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }
        else
        {
            return password;
        }
    }
}
