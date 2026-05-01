using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;

namespace TopLibraryManager.Services.Interfaces;

public interface IConsoleUIService
{
    /// <summary>
    /// Чтение пользовательского запроса из консоли
    /// </summary>
    /// <returns>Команда и аргументы к ней</returns>
    public CommandRequest? ReadCommandRequest();

    /// <summary>
    /// Запись строки в консоль
    /// </summary>
    /// <param name="line">Строка текста</param>
    public void WriteLine(string line);

    /// <summary>
    /// Запись строк в консоль. Используется для ответа на пользовательский запрос
    /// </summary>
    /// <param name="lines">Строки текста</param>
    public void WriteLines(string[] lines);

    /// <summary>
    /// Чтение строки из консоли
    /// </summary>
    /// <param name="prompt">Подсказка для пользователя</param>
    /// <returns>Введенная строка</returns>
    public string? ReadLine(string? prompt);

    /// <summary>
    /// Чтение пароля из консоли с маскировкой символов
    /// </summary>
    /// <param name="prompt">Подсказка для пользователя</param>
    /// <returns>Введенный пароль</returns>
    public string? ReadPassword(string? prompt);
}
