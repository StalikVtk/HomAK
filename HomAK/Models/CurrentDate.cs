using System.ComponentModel.DataAnnotations.Schema;

namespace HomAK.Models
{
  /// <summary>
  /// Установленная дата.
  /// </summary>
  [NotMapped]
  internal class CurrentDate
  {
    /// <summary>
    /// Установленная дата.
    /// </summary>
    private DateTime setDate;

    #region Методы

    /// <summary>
    /// Получить установленный месяц.
    /// </summary>
    /// <returns>Месяц в строковом представлении.</returns>
    public string GetCurrentMonth()
    {
      return setDate.ToString("MMMM");
    }

    /// <summary>
    /// Получить установленную дату.
    /// </summary>
    /// <returns>Установленная дату</returns>
    public DateTime GetSetDate()
    {
      return setDate;
    }

    /// <summary>
    /// Добавить месяц.
    /// </summary>
    /// <returns>Следующий месяц.</returns>
    public void AddMonth()
    {
      setDate = setDate.AddMonths(1);
    }

    /// <summary>
    /// Вычесть месяц.
    /// </summary>
    public void SubtractMonth()
    {
      setDate = setDate.AddMonths(-1);
    }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CurrentDate(DateTime setDate)
    {
      this.setDate = setDate;
    }
  }
}
