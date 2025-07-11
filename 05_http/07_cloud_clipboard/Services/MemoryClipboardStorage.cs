using _07_cloud_clipboard.Models;

namespace _07_cloud_clipboard.Services;

internal class MemoryClipboardStorage : IClipboardStorage
{
    private readonly List<ClipboardItem> history = new();
    private readonly object locker = new();
    private int maxHistory;

    public MemoryClipboardStorage(int maxHistory = 100)
    {
        this.maxHistory = maxHistory;
    }

    public IEnumerable<ClipboardItem> GetHistory()
    {
        lock (locker)
        {
            return history;
        }
    }

    public ClipboardItem? GetLastItem()
    {
        lock(locker)
        {
            return history.FirstOrDefault();
        }
    }

    public void SaveItem(string content)
    {
        lock (locker)
        {
            // TODO: refactoring: push to class "ClipboardItemCollection"
            history.Insert(0, new ClipboardItem { Content = content, CreatedAt = DateTime.Now });

            if (history.Count > maxHistory)
            {
                history.RemoveRange(maxHistory, history.Count - maxHistory);
            }
        }

    }
}
