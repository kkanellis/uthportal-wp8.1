﻿using UTHPortal.Models;

namespace UTHPortal.Common
{
    public interface IPushService
    {
        void Register(RestAPIItem item);
        void Unregister(RestAPIItem item);
        PushEvent IsRegistered(RestAPIItem item);
    }
}
