using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.View;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Windows;

namespace HomAK.ViewModels
{
  internal partial class CategoryViewModel : ObservableObject
  {

    #region Поля

    /// <summary>
    /// Список категорий.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Category>? categories;

    /// <summary>
    /// Выбранная категория.
    /// </summary>
    [ObservableProperty]
    private Category? selectedCategory;

    /// <summary>
    /// Новое имя категории.
    /// </summary>
    [ObservableProperty]
    private string? newCategoryName;

    /// <summary>
    /// Добавление категории.
    /// </summary>
    public IRelayCommand AddCommand { get; }

    /// <summary>
    /// Редактирование категории.
    /// </summary>
    public IRelayCommand EditCommand { get; }

    /// <summary>
    /// Удаление категории.
    /// </summary>
    public IRelayCommand DeleteCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Редактировать категорию.
    /// </summary>
    private void EditCategory()
    {
      if (SelectedCategory == null)
      {
        MessageBox.Show("Выберите категорию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      EditCategoryViewModel editCategoryViewModel = new EditCategoryViewModel(SelectedCategory);

      WindowEditCategory windowEditCategory = new WindowEditCategory(editCategoryViewModel);
      windowEditCategory.ShowDialog();

      LoadCategories();
    }

    /// <summary>
    /// Удалить категорию.
    /// </summary>
    private void DeleteCategory()
    {
      using var db = new ApplicationContext();
      db.Categories.Remove(selectedCategory);
      db.SaveChanges();

      Categories.Remove(selectedCategory);

      OnPropertyChanged(nameof(Categories));
    }

    /// <summary>
    /// Добавить категорию.
    /// </summary>
    private void AddCategory()
    {
      if (NewCategoryName.IsNullOrEmpty())
      {
        MessageBox.Show($"Название категории не может быть пустым!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      var category = new Category { Name = NewCategoryName.Trim() };

      using var db = new ApplicationContext();

      if (db.Categories.Any(c => c.Name == category.Name))
      {
        NewCategoryName = string.Empty;
        MessageBox.Show($"Категория '{category.Name}' уже существует", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      db.Categories.Add(category);
      db.SaveChanges();

      Categories.Add(category);

      NewCategoryName = string.Empty;

      OnPropertyChanged(nameof(Categories));
    }

    /// <summary>
    /// Загрузить категории.
    /// </summary>
    private void LoadCategories()
    {
      using var db = new ApplicationContext();
      var categoryList = db.Categories.ToList();

      Categories = new ObservableCollection<Category>(categoryList);
    }

    #endregion

    #region Конструкторы

    public CategoryViewModel()
    {
      Categories = new ObservableCollection<Category>();
      AddCommand = new RelayCommand(AddCategory);
      DeleteCommand = new RelayCommand(DeleteCategory);
      EditCommand = new RelayCommand(EditCategory);

      LoadCategories();
    }

    #endregion

  }
}
