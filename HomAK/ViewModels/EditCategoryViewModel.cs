using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomAK.DataAccess;
using HomAK.Models;
using System.Windows;

namespace HomAK.ViewModels
{
  internal partial class EditCategoryViewModel : ObservableObject
  {

    #region Поля

    [ObservableProperty]
    private Category selectedCategory;

    [ObservableProperty]
    private string newNameCategory;

    public RelayCommand SaveCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Сохранить категорию.
    /// </summary>
    private void SaveCategory()
    {
      if (string.IsNullOrEmpty(NewNameCategory))
      {
        MessageBox.Show("Укажите название!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }  

      using var db = new ApplicationContext();
      var category = db.Categories.Find(selectedCategory.Id);

      if (category.Id == selectedCategory.Id && category.Name == NewNameCategory)
      {
        MessageBox.Show("Категория уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      category.Name = NewNameCategory;
      db.SaveChanges();

      CloseWindow();
    }


    /// <summary>
    /// Закрыть окно редактирования категории.
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

    public EditCategoryViewModel(Category category)
    {
      this.SelectedCategory = category;
      this.NewNameCategory = SelectedCategory.Name;

      SaveCommand = new RelayCommand(SaveCategory);
    }

    #endregion

  }
}
