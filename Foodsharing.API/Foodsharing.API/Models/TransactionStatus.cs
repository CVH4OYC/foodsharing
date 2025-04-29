using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус обмена
/// </summary>
public class TransactionStatus : EntityBase
{   
    /// <summary>
    /// Название статуса транзакции
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Transaction
    /// </summary>
    public List<Transaction>? Transactions { get; set; }
}
