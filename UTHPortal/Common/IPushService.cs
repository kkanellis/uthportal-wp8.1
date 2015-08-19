using UTHPortal.Models;

namespace UTHPortal.Common
{
    public interface IPushService
    {
        void RegisterUrl(string url);
        void UnregisterUrl(string url);
        PushEvent IsRegistered(string url);
    }
}
