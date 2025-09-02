using HomAK.Models;

namespace HomAK.Service
{
  /// <summary>
  /// Сообщение добавления Count.
  /// </summary>
  internal class CountMessage
  {
    public Count Count { get; }

    public CountMessage(Count count)
    {
      this.Count = count;
    }
  }
}
