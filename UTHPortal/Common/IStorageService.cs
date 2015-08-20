using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public interface IStorageService
    {
        Task<string> LoadJSON(RestAPIItem item);
        Task SaveJSON(RestAPIItem item, string data);

        object GetSettingsEntry(string entryName);
        bool SetSettingsEntry(string entryName, object value);
    }
}
