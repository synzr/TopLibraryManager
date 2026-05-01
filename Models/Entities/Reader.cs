using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLibraryManager.Models.Entities;

public class Reader
{
    public int Id { get; set; }

    public string Fio { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime RegisteredAt { get; set; }
}
