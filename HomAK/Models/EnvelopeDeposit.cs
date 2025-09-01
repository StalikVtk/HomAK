using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Пополнение конверта.
  /// </summary>
  [Table("tb_envelopeDeposit")]
  internal class EnvelopeDeposit
  {
    /// <summary>
    /// Индификатор.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Дата.
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    public decimal Amount { get; set; }

    public Guid EnvelopeId { get; set; }

    public Envelope? Envelope { get; set; }
  }
}
