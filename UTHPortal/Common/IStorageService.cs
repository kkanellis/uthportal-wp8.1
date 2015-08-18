using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public interface IStorageService
    {
        Task<string> LoadJSON(string url);
        Task SaveJSON(string url, string data);

        object GetSettingsEntry(string entryName);
        bool SetSettingsEntry(string entryName, object value);
    }
}
