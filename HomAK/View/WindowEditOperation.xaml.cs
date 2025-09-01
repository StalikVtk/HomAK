using HomAK.ViewModels;
using System.Windows;

namespace HomAK.View
{
  /// <summary>
  /// Логика взаимодействия для WindowEditOperation.xaml
  /// </summary>
  public partial class WindowEditOperation : Window
  {
    internal WindowEditOperation(EditOperationViewModel editOperationViewModel)
    {
      InitializeComponent();
      DataContext = editOperationViewModel;
    }
  }
}
