using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.Service;
using System.Collections.ObjectModel;

namespace HomAK.ViewModels
{
  internal partial class AddOperationViewModel : ObservableObject
  {

    #region Поля

    private readonly CountViewModel count = new CountViewModel();

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

    public ObservableCollection<Count> Counts => count.Counts;

    public ObservableCollection<Category> Categories => category.Categories;

    public IRelayCommand AddCommand { get; }

    public IRelayCommand ClearCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Добавить операцию.
    /// </summary>
    public void AddOperation()
    {
      Operation operation = new Operation();

      operation.Id = new Guid();
      operation.CountId= selectedCount.Id;
      operation.DateOperation = operationDate;
      operation.CategoryId = selectedCategory.Id;
      operation.TypeOperation = TypeOperation(selectedTypeOperation);
      operation.Amount = amount;
      operation.Comment = comment;

      using var db = new ApplicationContext();
      db.Operations.Add(operation);
      db.SaveChanges();

      Messenger.Default.Send(new AddOperationMessage(operation));

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

    public AddOperationViewModel()
    {
      AddCommand = new RelayCommand(AddOperation);
      ClearCommand = new RelayCommand(ClearFields);
    }

    #endregion

  }
}
