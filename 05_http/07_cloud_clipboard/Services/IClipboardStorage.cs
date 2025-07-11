using _07_cloud_clipboard.Models;

namespace _07_cloud_clipboard.Services;

internal interface IClipboardStorage
{
    ClipboardItem? GetLastItem();
    void SaveItem(string content);
    IEnumerable<ClipboardItem> GetHistory();
}
