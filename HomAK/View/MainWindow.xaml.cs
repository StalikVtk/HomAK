using HomAK.Resources.ResourcseWindow;
using HomAK.ViewModels;
using System.Windows;
using HomAK.View;

namespace HomAK
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainWindowViewModel();
    }

    private void MenuItem_Click_Category(object sender, RoutedEventArgs e)
    {
      WindowCategory WindowCategory = new WindowCategory();
      WindowCategory.Show();
    }

    private void MenuItem_Click_Count(object sender, RoutedEventArgs e)
    {
      WindowCount WindowCount = new WindowCount();
      WindowCount.Show();
    }

    private void ExitProgramm(object sender, RoutedEventArgs e)
    {
      var InExit = MessageBox.Show("Выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (InExit == MessageBoxResult.Yes)
      {
        Application.Current.Shutdown();
      }
    }
  }
}