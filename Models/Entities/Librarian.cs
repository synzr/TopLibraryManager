using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLibraryManager.Models.Entities;

public class Librarian
{
    public int Id { get; set; }

    public string Fio { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
}
