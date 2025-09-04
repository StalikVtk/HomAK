using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace HomAK.ViewModels
{
  internal partial class AddOperationViewModel : ObservableObject
  {

    #region Поля

    private readonly CategoryViewModel category = new CategoryViewModel();

    [ObservableProperty]
    private DateTime operationDate = DateTime.Now;

    [ObservableProperty]
    private decimal amount;

    [ObservableProperty]
    private Count selectedCount;

    [ObservableProperty]
    private Category selectedCategory;

    [ObservableProperty]
    private string selectedTypeOperation;

    [ObservableProperty]
    private string comment;

    public ObservableCollection<Count> Counts { get; }

    public ObservableCollection<Category> Categories => category.Categories;

    public RelayCommand AddCommand { get; }

    public RelayCommand ClearCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Добавить операцию.
    /// </summary>
    public void AddOperation()
    {
      if (SelectedCount == null)
      {
        MessageBox.Show("Выберите счет!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (SelectedCategory == null)
      {
        MessageBox.Show("Выберите категорию!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (Amount <= 0)
      {
        MessageBox.Show("Сумма должна быть больше 0", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (TypeOperation(SelectedTypeOperation) == OperationType.Expense && 
        SelectedCount.CurrentAmmount - Amount < decimal.Zero)
      { 
        MessageBox.Show("Недостаточно средств на счете!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      Operation operation = new Operation();

      operation.Id = new Guid();
      operation.CountId= SelectedCount.Id;
      operation.DateOperation = OperationDate;
      operation.CategoryId = SelectedCategory.Id;
      operation.TypeOperation = TypeOperation(SelectedTypeOperation);
      operation.Amount = Amount;
      operation.Comment = Comment;

      using var db = new ApplicationContext();
      db.Operations.Add(operation);
      db.SaveChanges();

      Messenger.Default.Send(new OperationMessage(operation));

      ClearFields();
    }

    /// <summary>
    /// Очистить поля.
    /// </summary>
    public void ClearFields()
    {
      SelectedCount = null;
      SelectedCategory = null;
      SelectedTypeOperation = null;
      Amount = decimal.Zero;
      SelectedTypeOperation = null;
      Comment = string.Empty;
      OperationDate = DateTime.Now;
    }

    /// <summary>
    /// Тип операции.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Тип операции из OperationType.</returns>
    private OperationType TypeOperation(string name)
    {
      return name == "Приход" ? OperationType.Income : OperationType.Expense;
    }

    #endregion

    #region Конструктор

    public AddOperationViewModel(ObservableCollection<Count> counts)
    {
      this.Counts = counts;
      this.AddCommand = new RelayCommand(AddOperation);
      this.ClearCommand = new RelayCommand(ClearFields);
    }

    #endregion

  }
}
