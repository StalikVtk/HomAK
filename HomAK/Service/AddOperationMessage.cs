using HomAK.Models;

namespace HomAK.Service
{
  /// <summary>
  /// Сообщение добавления Operation.
  /// </summary>
  internal class AddOperationMessage
  {
    public Operation Operation { get; }

    public AddOperationMessage(Operation operation)
    {
      this.Operation = operation;
    }
  }
}
