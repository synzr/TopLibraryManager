using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;

namespace TopLibraryManager.Services.Interfaces;

public interface ICommandProcessorService
{
    /// <summary>
    /// Обработка команд
    /// </summary>
    /// <param name="commandRequest">Запрос команды</param>
    /// <returns>Продолжить ли цикл программу?</returns>
    public bool ProcessCommand(CommandRequest commandRequest);
}
