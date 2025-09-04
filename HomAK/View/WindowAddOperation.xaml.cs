using HomAK.ViewModels;
using System.Windows;

namespace HomAK.View
{
  /// <summary>
  /// Логика взаимодействия для WindowOperation.xaml
  /// </summary>
  public partial class WindowAddOperation : Window
  {
    internal WindowAddOperation(AddOperationViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}
