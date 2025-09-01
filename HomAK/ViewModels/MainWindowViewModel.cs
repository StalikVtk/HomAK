using System.Collections.ObjectModel;
using System.Windows.Input;
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

    public CountViewModel CountVM { get; }

    public ObservableCollection<Count> Counts => count.Counts;

    public CurrentDateViewModel CurrentDateViewModel
    {
      get => currentDate;
      set => SetProperty(ref currentDate, value);
    }

    public string CurrentMonthName => CurrentDateViewModel.CurrentMonthName;

    public ICommand PreviousMonthCommand => CurrentDateViewModel.PreviousMonthCommand;

    public ICommand NextMonthCommand => CurrentDateViewModel.NextMonthCommand;

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
        .OrderByDescending(o => o.DateOperation)
        .ToList();

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

    #endregion

    #region Конструкторы

    public MainWindowViewModel()
    {
      CurrentDateViewModel = new CurrentDateViewModel(DateTime.Now);
      DeleteCommand = new RelayCommand(DeleteOperation);
      EditCommand = new RelayCommand(EditOperation);

      Messenger.Default.Register<AddOperationMessage>(this, message =>
      {
        UpdateViewOperations();
      });

      Messenger.Default.Register<AddCountMessage>(this, message =>
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
