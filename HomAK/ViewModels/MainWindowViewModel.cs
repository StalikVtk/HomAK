using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.Models;
using HomAK.Service;
using HomAK.View;

namespace HomAK.ViewModels
{
  internal partial class MainWindowViewModel : ObservableObject
  {

    #region Поля

    private readonly CountViewModel count = new CountViewModel();

    private currentDateViewModel currentDate;

    [ObservableProperty]
    private OperationViewModel operationViewModel;

    [ObservableProperty]
    private Operation selectedOperation;

    [ObservableProperty]
    private Count selectedCount;

    public ObservableCollection<Count> Counts => count.Counts;

    public currentDateViewModel CurrentDateViewModel
    {
      get => currentDate;
      set => SetProperty(ref currentDate, value);
    }

    public string CurrentMonthName => CurrentDateViewModel.CurrentMonthName;

    public RelayCommand PreviousMonthCommand { get; }

    public RelayCommand NextMonthCommand { get; }

    public RelayCommand DeleteCommand { get; }

    public RelayCommand EditCommand { get; }

    public RelayCommand AddCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Загрузить операции по видам.
    /// </summary>
    private void LoadAllOperation()
    {
      OperationViewModel = new OperationViewModel(SelectedCount, CurrentDateViewModel);
      GetCurrentBalance();
    }

    /// <summary>
    /// Получить текущий баланс счета.
    /// </summary>
    private void GetCurrentBalance()
    {
      var TempCounts = Counts.ToList();
      Counts.Clear();
      for (var itemCount = 0; itemCount < TempCounts.Count; itemCount++)
      {
        var CurrentCount = TempCounts[itemCount];
        var OperationsCount = new OperationViewModel(CurrentCount, CurrentDateViewModel);
        CurrentCount.CurrentAmmount = CurrentCount.Ammount + 
          (OperationsCount.SumIncome ?? 0) - (OperationsCount.SumExpense ?? 0);

        Counts.Add(CurrentCount);
      }
      OnPropertyChanged(nameof(Counts));
    }

    /// <summary>
    /// Редактировать текущую операцию.
    /// </summary>
    private void EditCurrentOperation()
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

      LoadAllOperation();
    }

    /// <summary>
    /// Добавить операцию.
    /// </summary>
    private void AddOperation()
    {
      var addOperationViewModel = new AddOperationViewModel(Counts);
      WindowAddOperation windowAddOperation = new WindowAddOperation(addOperationViewModel);
      windowAddOperation.Owner = Application.Current.MainWindow;
      windowAddOperation.ShowDialog();

      GetCurrentBalance();
    }

    /// <summary>
    /// Удалить текущую операцию.
    /// </summary>
    private void DeleteCurrentOperation()
    {
      if (SelectedOperation == null)
      {
        MessageBox.Show("Выберите операцию!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      OperationViewModel.DeleteOperation(SelectedOperation);

      LoadAllOperation();
      GetCurrentBalance();
    }

    /// <summary>
    /// Переключиться на следующий месяц.
    /// </summary>
    private void NextMonth()
    {
      CurrentDateViewModel.NextMonth();
      LoadAllOperation();
    }

    /// <summary>
    /// Переключиться на предыдущий месяц.
    /// </summary>
    private void PreviousMonth()
    {
      CurrentDateViewModel.PreviousMonth();
      LoadAllOperation();
    }

    /// <summary>
    /// Устновить выбранный счет.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    partial void OnSelectedCountChanged(Count? oldValue, Count? newValue)
    {
      if (newValue != null)
      {
        LoadAllOperation();
      }
    }

    #endregion

    #region Конструкторы

    public MainWindowViewModel()
    {
      this.CurrentDateViewModel = new currentDateViewModel(DateTime.Now);
      this.SelectedCount = count.Counts.FirstOrDefault();

      GetCurrentBalance();

      this.OperationViewModel = new OperationViewModel(SelectedCount, CurrentDateViewModel);

      this.AddCommand = new RelayCommand(AddOperation);
      this.DeleteCommand = new RelayCommand(DeleteCurrentOperation);
      this.EditCommand = new RelayCommand(EditCurrentOperation);
      this.PreviousMonthCommand = new RelayCommand(PreviousMonth);
      this.NextMonthCommand = new RelayCommand(NextMonth);

      Messenger.Default.Register<OperationMessage>(this, message =>
      {
        LoadAllOperation();
      });

      Messenger.Default.Register<CountMessage>(this, message =>
      {
        LoadAllOperation();
      });

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
