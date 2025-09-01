using HomAK.Models;

namespace HomAK.Service
{
  /// <summary>
  /// Сообщение добавления Count.
  /// </summary>
  internal class AddCountMessage
  {
    public Count Count { get; }

    public AddCountMessage(Count count)
    {
      this.Count = count;
    }
  }
}
