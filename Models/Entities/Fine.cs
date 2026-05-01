using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models.Enums;

namespace TopLibraryManager.Models.Entities;

public class Fine
{
    public int Id { get; set; }

    // Внешний ключ на выдачу
    public int LoanId { get; set; }
    public Loan Loan { get; set; }

    // Стоимость штрафа за день (в копейках/центах)
    public int PricePerDay { get; set; }

    // Общая сумма штрафа (рассчитывается автоматически)
    public int Amount { get; set; }

    // Статус штрафа
    public FineStatus Status { get; set; }

    // Дата создания штрафа
    public DateTime CreatedAt { get; set; }

    // Дата оплаты штрафа
    public DateTime? PaidAt { get; set; }
}
