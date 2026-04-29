using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLibraryManager.Models;

public class CommandRequest(string command, string[] args)
{
    /// <summary>
    /// Пользовательская команда
    /// </summary>
    public string Command { get; } = command;

    /// <summary>
    /// Аргументы команды
    /// </summary>
    public string[] Args { get; } = args;
}
