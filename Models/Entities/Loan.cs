using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models.Enums;

namespace TopLibraryManager.Models.Entities;

public class Loan
{
    public int Id { get; set; }

    // Внешние ключи
    public int BookId { get; set; }
    public int ReaderId { get; set; }
    public int LibrarianId { get; set; }

    // Навигационные свойства
    public Book Book { get; set; }
    public Reader Reader { get; set; }
    public Librarian Librarian { get; set; }

    // Дата и время выдачи
    public DateTime IssuedAt { get; set; }

    // Планируемая дата возврата
    public DateOnly ReturnAt { get; set; }

    // Фактическая дата возврата (null если книга еще не возвращена)
    public DateTime? ReturnedAt { get; set; }

    // Статус выдачи
    public LoanStatus Status { get; set; }

    // Рассчитанное свойство: количество дней просрочки
    public int DaysOverdue 
    {
        get
        {
            if (ReturnedAt.HasValue)
                return Math.Max(0, (ReturnedAt.Value.Date - ReturnAt.ToDateTime(TimeOnly.MinValue)).Days);
            else
                return Math.Max(0, (DateTime.Today - ReturnAt.ToDateTime(TimeOnly.MinValue)).Days);
        }
    }
}
