using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomAK.DataAccess;
using HomAK.Models;
using System.Collections.ObjectModel;
using HomAK.View;
using GalaSoft.MvvmLight.Messaging;
using HomAK.Service;

namespace HomAK.ViewModels
{
  internal partial class CountViewModel : ObservableObject
  {

    #region Поля

    /// <summary>
    /// Список счетов.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Count> counts;

    /// <summary>
    /// Выбранный счет.
    /// </summary>
    [ObservableProperty]
    private Count selectedCount;

    public IRelayCommand WindowAddCount { get; }

    /// <summary>
    /// Команнда редактирования.
    /// </summary>
    public IRelayCommand EditCommand { get; }

    /// <summary>
    /// Команда удаления.
    /// </summary>
    public IRelayCommand DeleteCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Загрузить счета.
    /// </summary>
    private void LoadCounts()
    {
      using var db = new ApplicationContext();
      var countList = db.Counts.ToList();

      Counts = new ObservableCollection<Count>(countList);
    }

    /// <summary>
    /// Удалить счет.
    /// </summary>
    private void DeleteCount()
    {
      using var db = new ApplicationContext();
      db.Remove(selectedCount);
      db.SaveChanges();

      Counts.Remove(selectedCount);

      OnPropertyChanged(nameof(Counts));
      OnPropertyChanged(nameof(MainWindowViewModel));
    }

    /// <summary>
    /// Редактировать счет.
    /// </summary>
    private void EditCount()
    {
      using var db = new ApplicationContext();
      var countSearchBd = db.Counts.Find(selectedCount.Id);

      countSearchBd.Name = selectedCount.Name;
      countSearchBd.Number = selectedCount.Number;
      countSearchBd.Ammount = selectedCount.Ammount;

      db.SaveChanges();

      OnPropertyChanged(nameof(Counts));
    }

    /// <summary>
    /// Вызвать окно для добавление счета.
    /// </summary>
    private void ShowWindowAddCount()
    {
      WindowAddCount windowAddCount = new WindowAddCount();
      windowAddCount.Show();
    }

    #endregion

    #region Конструкторы

    public CountViewModel()
    {
      Counts = new ObservableCollection<Count>();
      WindowAddCount = new RelayCommand(ShowWindowAddCount);
      EditCommand = new RelayCommand(EditCount);
      DeleteCommand = new RelayCommand(DeleteCount);

      Messenger.Default.Register<AddCountMessage>(this, message =>
      {
        LoadCounts();
      });
     
      LoadCounts();
    }

    #endregion

  }
}
