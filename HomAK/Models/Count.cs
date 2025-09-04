using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Счет.
  /// </summary>
  [Table("tb_count")]
  internal class Count
  {
    /// <summary>
    /// Индификатор.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }

    /// <summary>
    /// Баланс.
    /// </summary>
    [Required]
    public decimal Ammount { get; set; }

    [NotMapped]
    public decimal CurrentAmmount { get; set; }

    /// <summary>
    /// Номер.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? Number { get; set; }

    /// <summary>
    /// Операции.
    /// </summary>
    public List<Operation> Operations { get; set; } = new();

    /// <summary>
    /// Конверты.
    /// </summary>
    public List<Envelope> Envelopes { get; set; } = new();

  }
}
