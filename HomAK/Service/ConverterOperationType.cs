using HomAK.Models;
using System.Globalization;
using System.Windows.Data;

namespace HomAK.Service
{
  /// <summary>
  /// Конвертер типа операции.
  /// </summary>
  internal class ConverterOperationType : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is OperationType operationType)
      {
        return operationType switch
        {
          OperationType.Income => "Приход",
          OperationType.Expense => "Расход",
          _ => throw new NotImplementedException(),
        };
      }
      return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
