using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.Service;
using HomAK.View;
using Microsoft.EntityFrameworkCore;

namespace HomAK.ViewModels
{
  internal partial class MainWindowViewModel : ObservableObject
  {

    #region Поля

    private readonly CountViewModel count = new CountViewModel();

    private CurrentDateViewModel currentDate;

    [ObservableProperty]
    private ObservableCollection<Operation>? operations;

    [ObservableProperty]
    private ObservableCollection<Operation>? operationsExpense;

    [ObservableProperty]
    private ObservableCollection<Operation>? operationsIncome;

    [ObservableProperty]
    private Operation selectedOperation;

    [ObservableProperty]
    private decimal? itogIncome;

    [ObservableProperty]
    private decimal? itogeExpense;

    [ObservableProperty]
    private Count selectedCount;

    public ObservableCollection<Count> Counts => count.Counts;

    public CurrentDateViewModel CurrentDateViewModel
    {
      get => currentDate;
      set => SetProperty(ref currentDate, value);
    }

    public string CurrentMonthName => CurrentDateViewModel.CurrentMonthName;

    public RelayCommand PreviousMonthCommand { get; }

    public RelayCommand NextMonthCommand { get; }

    public RelayCommand DeleteCommand { get; }

    public RelayCommand EditCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Загрузить операции.
    /// </summary>
    private void LoadAllOperetion()
    {
      using var db = new ApplicationContext();

      var operationsList = db.Operations
        .Include(o => o.Count)
        .Include(o => o.Category)
        .Where(o => o.DateOperation >= CurrentDateViewModel.GetFirstDayMonth() 
          && o.DateOperation <= CurrentDateViewModel.GetLastDayMonth()
          && o.DateOperation.Month == CurrentDateViewModel.GetCurrentMonth())
        .OrderByDescending(o => o.DateOperation)
        .ToList();

      if (SelectedCount != null)
      {
        operationsList = operationsList
          .Where(o => o.Count.Id == SelectedCount.Id)
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

      ItogeExpense = OperationsExpense.Sum(o => o.Amount);
    }

    /// <summary>
    /// Загрузить операции с типом Приход.
    /// </summary>
    private void LoadOperationIncome() 
    {
      OperationsIncome = new ObservableCollection<Operation>(
        Operations.Where(o => o.TypeOperation == OperationType.Income));

      ItogIncome = OperationsIncome.Sum(o => o.Amount);
    }

    /// <summary>
    /// Обновить представление главной страницы.
    /// </summary>
    private void UpdateViewOperations()
    {
      LoadAllOperetion();
    }

    /// <summary>
    /// Редактировать операцию.
    /// </summary>
    private void EditOperation()
    {
      if (SelectedOperation == null)
      {
        MessageBox.Show("Выберите операцию!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      EditOperationViewModel editOperationViewModel = new EditOperationViewModel(SelectedOperation);

      WindowEditOperation windowEditOperation = new WindowEditOperation(editOperationViewModel);

      windowEditOperation.ShowDialog();
    }

    /// <summary>
    /// Удалить операцию.
    /// </summary>
    private void DeleteOperation()
    {
      if (SelectedOperation == null)
        return;

      using var db = new ApplicationContext();
      db.Operations.Remove(SelectedOperation);
      db.SaveChanges();

      Operations.Remove(SelectedOperation);

      UpdateViewOperations();
    }

    /// <summary>
    /// Переключиться на следующий месяц.
    /// </summary>
    private void NextMonth()
    {
      CurrentDateViewModel.NextMonth();
      LoadAllOperetion();
      UpdateViewOperations();
    }

    /// <summary>
    /// Переключиться на предыдущий месяц.
    /// </summary>
    private void PreviousMonth()
    {
      CurrentDateViewModel.PreviousMonth();
      LoadAllOperetion();
      UpdateViewOperations();
    }

    partial void OnSelectedCountChanged(Count? oldValue, Count? newValue)
    {
      if (newValue != null)
      {
        LoadAllOperetion();
      }
    }

    #endregion

    #region Конструкторы

    public MainWindowViewModel()
    {
      this.CurrentDateViewModel = new CurrentDateViewModel(DateTime.Now);
      this.DeleteCommand = new RelayCommand(DeleteOperation);
      this.EditCommand = new RelayCommand(EditOperation);
      this.PreviousMonthCommand = new RelayCommand(PreviousMonth);
      this.NextMonthCommand = new RelayCommand(NextMonth);
      this.SelectedCount = count.Counts.FirstOrDefault();

      Messenger.Default.Register<OperationMessage>(this, message =>
      {
        UpdateViewOperations();
      });

      Messenger.Default.Register<CountMessage>(this, message =>
      {
        OnPropertyChanged(nameof(Counts));
      });

      LoadAllOperetion();

      CurrentDateViewModel.PropertyChanged += (s, e) =>
      {
        if (e.PropertyName == nameof(CurrentDateViewModel.CurrentMonthName))
        {
          OnPropertyChanged(nameof(CurrentMonthName));
        }
      };
    }

    #endregion

  }
}
