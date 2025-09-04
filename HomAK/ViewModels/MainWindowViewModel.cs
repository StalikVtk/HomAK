using System.Collections.ObjectModel;
using System.Windows;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.Models;
using HomAK.Service;
using HomAK.View;
using Microsoft.Win32;

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

    public RelayCommand ExportToExcelCommand { get; }

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

    private void ExportToExcelOperation()
    {
      if (OperationViewModel == null ||
        OperationViewModel.OperationsExpense?.Count == 0 &&
        OperationViewModel.OperationsIncome?.Count == 0)
      {
        MessageBox.Show("Нет операций для экспорта", "Ошибка",
                   MessageBoxButton.OK, MessageBoxImage.Stop);
        return;
      }

      var saveFile = new SaveFileDialog
      {
        Filter = "Excel Files|*.xlsx",
        FileName = $"Операции_{CurrentDateViewModel.CurrentMonthName}.xlsx",
        Title = "Сохранить файл"
      };

      if (saveFile.ShowDialog() == true)
      {
        CreateExcelFile(saveFile.FileName);
        MessageBox.Show("Даннные экспортированы в Excel", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void CreateExcelFile(string filePath)
    {
      using var workBook = new XLWorkbook();

      if (OperationViewModel.OperationsExpense?.Count > 0)
      {
        CreateWorkSheet(workBook, "Расходы", OperationViewModel.OperationsExpense);
      }

      if (OperationViewModel.OperationsIncome?.Count > 0)
      {
        CreateWorkSheet(workBook, "Доходы", OperationViewModel.OperationsIncome);
      }

      CreatSumWorkSheet(workBook);

      workBook.SaveAs(filePath);
    }

    private void CreateWorkSheet(XLWorkbook workbook, string workSheetName, ObservableCollection<Operation> operations)
    {
      var workSheet = workbook.Worksheets.Add(workSheetName);

      workSheet.Cell(1, 1).Value = "Дата операции";
      workSheet.Cell(1, 2).Value = "Сумма";
      workSheet.Cell(1, 3).Value = "Категория";
      workSheet.Cell(1, 4).Value = "Счет";
      workSheet.Cell(1, 5).Value = "Вид операции";
      workSheet.Cell(1, 6).Value = "Комментарий";

      for (int i = 0; i < operations.Count; i++)
      {
        var curOperation = operations[i];
        var row = i + 2;

        workSheet.Cell(row, 1).Value = curOperation.DateOperation;
        workSheet.Cell(row, 1).Style.DateFormat.Format = "dd.MM.yyyy";

        workSheet.Cell(row, 2).Value = curOperation.Amount;
        workSheet.Cell(row, 3).Value = curOperation.Category?.Name;
        workSheet.Cell(row, 4).Value = curOperation.Count?.Number;
        workSheet.Cell(row, 5).Value = curOperation.TypeOperation == OperationType.Income ? "Приход" : "Расход";
        workSheet.Cell(row, 6).Value = curOperation.Comment;

        workSheet.Columns().AdjustToContents();
      }
    }

    private void CreatSumWorkSheet(XLWorkbook workbook)
    {
      var workSheet = workbook.Worksheets.Add("Итоги");

      workSheet.Cell(1, 1).Value = "Отчет";

      workSheet.Cell(3, 1).Value = "Месяц";
      workSheet.Cell(3, 2).Value = $"{CurrentDateViewModel.CurrentMonthName}";

      workSheet.Cell(5, 1).Value = "Общие расходы";
      workSheet.Cell(5, 2).Value = OperationViewModel.SumExpense ?? 0;

      workSheet.Cell(7, 1).Value = "Общие доходы";
      workSheet.Cell(7, 2).Value = OperationViewModel.SumIncome ?? 0;

      workSheet.Columns().AdjustToContents();
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
      this.ExportToExcelCommand = new RelayCommand(ExportToExcelOperation);

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
