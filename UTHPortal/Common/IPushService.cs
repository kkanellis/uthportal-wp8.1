namespace UTHPortal.Common
{
    public interface IPushService
    {
        void RegisterUrl(string url);
        void UnregisterUrl(string url);

    }
}
