using HomAK.Models;

namespace HomAK.Service
{
  /// <summary>
  /// Сообщение добавления Operation.
  /// </summary>
  internal class OperationMessage
  {
    public Operation Operation { get; }

    public OperationMessage(Operation operation)
    {
      this.Operation = operation;
    }
  }
}
