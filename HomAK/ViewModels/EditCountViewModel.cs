using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomAK.Models;

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

    private void EditCount()
    { 
      
    }

    private void ClearFields()
    { 
      
    }

    #endregion

    #region Конструкторы

    public EditCountViewModel()
    {
      this.EditCommand = new RelayCommand(EditCount);
      this.ClearCommand = new RelayCommand(ClearFields);
    }

    #endregion

  }
}
