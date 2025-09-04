using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.View;
using Microsoft.EntityFrameworkCore;

namespace HomAK.ViewModels
{
  internal partial class OperationViewModel : ObservableObject
  {

    #region Поля

    [ObservableProperty]
    private ObservableCollection<Operation>? operations;

    [ObservableProperty]
    private ObservableCollection<Operation>? operationsExpense;

    [ObservableProperty]
    private ObservableCollection<Operation>? operationsIncome;

    [ObservableProperty]
    private decimal? sumIncome;

    [ObservableProperty]
    private decimal? sumExpense;

    #endregion

    #region Методы

    /// <summary>
    /// Загрузить все операции.
    /// </summary>
    /// <param name="selectedCount"></param>
    /// <param name="currentDateViewModel"></param>
    private void LoadAllOperetion(Count selectedCount, currentDateViewModel currentDateViewModel)
    {
      using var db = new ApplicationContext();

      var operationsList = db.Operations
        .Include(o => o.Count)
        .Include(o => o.Category)
        .Where(o => o.DateOperation >= currentDateViewModel.GetFirstDayMonth()
          && o.DateOperation <= currentDateViewModel.GetLastDayMonth()
          && o.DateOperation.Month == currentDateViewModel.GetCurrentMonth())
        .OrderByDescending(o => o.DateOperation)
        .ToList();

      if (selectedCount != null)
      {
        operationsList = operationsList
          .Where(o => o.Count.Id == selectedCount.Id)
          .ToList();
      }

      Operations = new ObservableCollection<Operation>(operationsList);

      LoadOperationExpense();
      LoadOperationIncome();
    }

    /// <summary>
    /// Загрузить операции с типом Расход.
    /// </summary>
    private void LoadOperationExpense()
    {
      OperationsExpense = new ObservableCollection<Operation>(
        Operations.Where(o => o.TypeOperation == OperationType.Expense));

      GetSumExpense();
    }

    /// <summary>
    /// Загрузить операции с типом Приход.
    /// </summary>
    private void LoadOperationIncome()
    {
      OperationsIncome = new ObservableCollection<Operation>(
        Operations.Where(o => o.TypeOperation == OperationType.Income));

      GetSumIncome();
    }

    /// <summary>
    /// Получить сумму операций с типом расход.
    /// </summary>
    private void GetSumExpense()
    {
      SumExpense = OperationsExpense.Sum(o => o.Amount);
    }

    /// <summary>
    /// Получить сумму операций с типом приход.
    /// </summary>
    private void GetSumIncome()
    {
      SumIncome = OperationsIncome.Sum(o => o.Amount);
    }

    /// <summary>
    /// Удалить операцию.
    /// </summary>
    public void DeleteOperation(Operation operation)
    {
      using var db = new ApplicationContext();
      db.Operations.Remove(operation);
      db.SaveChanges();
    }

    #endregion

    public OperationViewModel(Count selectedCount, currentDateViewModel currentDateViewModel)
    {
      LoadAllOperetion(selectedCount, currentDateViewModel);
    }
  }
}
