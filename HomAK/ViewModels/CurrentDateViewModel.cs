using HomAK.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace HomAK.ViewModels
{
  internal partial class CurrentDateViewModel : ObservableObject
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
    [RelayCommand]
    private void NextMonth()
    {
      currentDate.AddMonth();
      UpdateDisplayData();
    }

    /// <summary>
    /// Предыдущий месяц.
    /// </summary>
    [RelayCommand]
    private void PreviousMonth()
    {
      currentDate.SubtractMonth();
      UpdateDisplayData();
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

    public CurrentDateViewModel(DateTime initDate)
    {
      currentDate = new CurrentDate(initDate);
      UpdateDisplayData();
    }

    #endregion
  }
}
