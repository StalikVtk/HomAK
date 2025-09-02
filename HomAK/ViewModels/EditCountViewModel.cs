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
  partial class EditCountViewModel : ObservableObject
  {

    #region Поля

    [ObservableProperty]
    private Count selectedCount;

    [ObservableProperty]
    private string newNameCount;

    [ObservableProperty]
    private string newNumberCount;

    [ObservableProperty]
    private decimal newAmountCount;

    public RelayCommand EditCommand { get; }

    public RelayCommand ClearCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Редактировать счет.
    /// </summary>
    private void EditCount()
    {
      using var db = new ApplicationContext();
      var countEdit = db.Counts.Find(SelectedCount.Id);

      if (countEdit.Name != NewNameCount.Trim() && 
        db.Counts.Any(c => c.Name == NewNameCount))
      {
        MessageBox.Show("Счет с таким наименованием уже существует!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (countEdit.Number != NewNumberCount.Trim() && 
        db.Counts.Any(c => c.Number == NewNumberCount))
      {
        MessageBox.Show("Счет с такми номером уже существует!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (NewAmountCount < decimal.Zero)
      {
        MessageBox.Show("Баланс не может быть отрицательным!", "Ошибка",
          MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      countEdit.Name = NewNameCount.Trim();
      countEdit.Number = NewNumberCount.Trim();
      countEdit.Ammount = NewAmountCount;

      db.SaveChanges();
      Messenger.Default.Send(new CountMessage(countEdit));
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

    public EditCountViewModel(Count selectedCount)
    {
      this.SelectedCount = selectedCount;
      this.NewAmountCount = selectedCount.Ammount;
      this.NewNameCount = selectedCount.Name;
      this.NewNumberCount = selectedCount.Number;
      this.EditCommand = new RelayCommand(EditCount);
    }

    #endregion

  }
}
