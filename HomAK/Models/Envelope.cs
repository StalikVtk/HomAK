using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Конверт.
  /// </summary>
  [Table ("tb_envelope")]
  internal class Envelope
  {
    /// <summary>
    /// Индификатор.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    [Required]
    public DateTime DateCreate { get; set; }

    /// <summary>
    /// Баланс.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// Процент от общего бюджета.
    /// </summary>
    [Required]
    [MaxLength(3)]
    public decimal Percent { get; set; }

    public Guid CountId { get; set; }

    public Count? Count { get; set; }

    public List<EnvelopeDeposit>? EnvelopeDeposits { get; set; }
  }
}
