using HomAK.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HomAK.ViewModels
{
  internal partial class currentDateViewModel : ObservableObject
  {

    #region Поля

    private readonly CurrentDate currentDate;

    [ObservableProperty]
    private string currentMonthName;

    #endregion

    #region Методы

    /// <summary>
    /// Обновить дату.
    /// </summary>
    private void UpdateDisplayData()
    {
      CurrentMonthName = currentDate.GetCurrentMonth();
    }

    /// <summary>
    /// Следующий месяц.
    /// </summary>
    public void NextMonth()
    {
      currentDate.AddMonth();
      UpdateDisplayData();
    }

    /// <summary>
    /// Предыдущий месяц.
    /// </summary>
    public void PreviousMonth()
    {
      currentDate.SubtractMonth();
      UpdateDisplayData();
    }

    /// <summary>
    /// Получить первый день установленного месяца.
    /// </summary>
    /// <returns>Первый день установленного месяца</returns>
    public DateTime GetFirstDayMonth()
    {
      var date = currentDate.GetSetDate();
      return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Получить последний день установленного месяца.
    /// </summary>
    /// <returns>Последний день установленного месяца</returns>
    public DateTime GetLastDayMonth()
    {
      var date = currentDate.GetSetDate();
      return date.AddMonths(1).AddDays(-1);
    }

    /// <summary>
    /// Получить месяц.
    /// </summary>
    /// <returns>Порядковое число установленного месяца.</returns>
    public int GetCurrentMonth()
    {
      return currentDate.GetSetDate().Month;
    }

    #endregion

    #region Конструкторы

    public currentDateViewModel(DateTime initDate)
    {
      currentDate = new CurrentDate(initDate);
      UpdateDisplayData();
    }

    #endregion
  }
}
