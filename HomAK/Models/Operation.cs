using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Операция.
  /// </summary>
  [Table("tb_operation")]
  internal class Operation
  {
    /// <summary>
    /// Инидификатор.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Дата операции.
    /// </summary>
    [Required]
    public DateTime DateOperation { get; set; }
    
    /// <summary>
    /// Тип опеарции.
    /// </summary>
    [Required]
    public OperationType TypeOperation { get; set;}

    /// <summary>
    /// Сумма.
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Комментарий.
    /// </summary>
    [MaxLength(50)]
    public string? Comment { get; set; }


    /// <summary>
    /// Внешний ключ.
    /// </summary>
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    /// <summary>
    /// Id счета
    /// </summary>
    public Guid CountId { get; set; }

    /// <summary>
    /// Счет
    /// </summary>
    public Count? Count { get; set; }
  }
}
