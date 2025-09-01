using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Messaging;
using HomAK.DataAccess;
using HomAK.Models;
using HomAK.Service;

namespace HomAK.ViewModels
{
  internal partial class AddCountViewModel : ObservableObject
  {

    #region Поля

    /// <summary>
    /// Новый счет.
    /// </summary>
    [ObservableProperty]
    private Count newCount;

    /// <summary>
    /// Команда добавить счет.
    /// </summary>
    public RelayCommand AddCommand { get; }

    /// <summary>
    /// Команда очистить поля.
    /// </summary>
    public RelayCommand ClearCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Добавить счет.
    /// </summary>
    private void AddCount()
    {
      var count = new Count 
      { 
        Name = NewCount.Name.Trim(), 
        Number = NewCount.Number.Trim(), 
        Ammount = NewCount.Ammount
      };

      using var db = new ApplicationContext();
      db.Counts.Add(count);
      db.SaveChanges();

      Messenger.Default.Send(new AddCountMessage(count));
      
      ClearFields();
    }

    /// <summary>
    /// Очистить поля.
    /// </summary>
    public void ClearFields()
    {
      NewCount = new Count();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструтктор.
    /// </summary>
    public AddCountViewModel()
    {
      AddCommand = new RelayCommand(AddCount);
      ClearCommand = new RelayCommand(ClearFields);
      NewCount = new Count();
    }

    #endregion

  }
}
