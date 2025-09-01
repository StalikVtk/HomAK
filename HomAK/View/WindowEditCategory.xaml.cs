using HomAK.ViewModels;
using System.Windows;

namespace HomAK.View
{
  /// <summary>
  /// Логика взаимодействия для WindowEditCategory.xaml
  /// </summary>
  public partial class WindowEditCategory : Window
  {
    internal WindowEditCategory(EditCategoryViewModel editCategoryViewModel)
    {
      InitializeComponent();
      DataContext = editCategoryViewModel;
    }
  }
}
