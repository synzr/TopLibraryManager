namespace TopLibraryManager.Models.Enums;

public enum LoanStatus
{
    /// <summary>
    /// Книга выдана (активная выдача)
    /// </summary>
    Active,
    
    /// <summary>
    /// Книга возвращена вовремя
    /// </summary>
    Returned,
    
    /// <summary>
    /// Книга просрочена (но еще не возвращена)
    /// </summary>
    Overdue,
    
    /// <summary>
    /// Книга возвращена (может быть с просрочкой)
    /// </summary>
    Completed
}