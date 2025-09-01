using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomAK.DataAccess;
using HomAK.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace HomAK.ViewModels
{
  partial class EditOperationViewModel : ObservableObject
  {

    #region Поля

    [ObservableProperty]
    private ObservableCollection<Category> categories;

    [ObservableProperty]
    private ObservableCollection<Count> counts;

    [ObservableProperty]
    private Operation selectedOperation;

    [ObservableProperty]
    private DateTime newDate;

    [ObservableProperty]
    private decimal newAmount;

    [ObservableProperty]
    private string newComment;

    [ObservableProperty]
    private OperationType newType;

    [ObservableProperty]
    private Category newCategory;

    [ObservableProperty]
    private Count newCount;

    public RelayCommand EditCommand { get; }

    #endregion

    #region Методы


    /// <summary>
    /// Редактировать операцию.
    /// </summary>
    private void EditOperation()
    {
      using var db = new ApplicationContext();
      var operation = db.Operations.Find(selectedOperation.Id);

      operation.DateOperation = NewDate;
      operation.Amount = NewAmount;
      operation.Comment = NewComment;
      operation.TypeOperation = NewType;
      operation.CategoryId = NewCategory.Id;
      operation.CountId = NewCount.Id;

      db.SaveChanges();
      CloseWindow();

    }

    /// <summary>
    /// Закрыть окно.
    /// </summary>
    private void CloseWindow()
    {
      foreach (Window window in Application.Current.Windows)
      {
        if (window.DataContext == this)
        {
          window.Close();
          break;
        }
      }
    }

    #endregion

    #region Конструкторы

    public EditOperationViewModel(Operation operation)
    {
      using var db = new ApplicationContext();
      var category = db.Categories;
      var count = db.Counts;

      this.Categories = new ObservableCollection<Category>(category);
      this.Counts = new ObservableCollection<Count>(count);

      this.NewDate = operation.DateOperation;
      this.NewAmount = (decimal)operation.Amount;
      this.NewComment = operation.Comment;
      this.NewType = operation.TypeOperation;

      this.NewCategory = Categories.FirstOrDefault(c => c.Id == operation.CategoryId);
      this.NewCount = Counts.FirstOrDefault(c => c.Id == operation.CountId);

      this.SelectedOperation = operation;

      this.EditCommand = new RelayCommand(EditOperation);
    }

    #endregion

  }
}
