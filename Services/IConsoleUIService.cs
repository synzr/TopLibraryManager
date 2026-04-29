using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;

namespace TopLibraryManager.Services;

public interface IConsoleUIService
{
    /// <summary>
    /// Чтение пользовательского запроса из консоли
    /// </summary>
    /// <returns>Команда и аргументы к ней</returns>
    public CommandRequest? ReadCommandRequest();

    /// <summary>
    /// Запись строк в консоль. Используется для ответа на пользовательский запрос
    /// </summary>
    /// <param name="lines">Строки текста</param>
    public void WriteLines(string[] lines);
}
