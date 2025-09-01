using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Категория.
  /// </summary>
  [Table("tb_operationCategory")]
  internal class Category
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
    /// Навигационное свойство.
    /// </summary>
    List<Operation> Operations { get; set; }
  }
}
