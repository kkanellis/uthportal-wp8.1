using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using UTHPortal.Models;

namespace UTHPortal.Common
{
    public class PushService : IPushService
    {
        public const string notificationSettingsStr = "NotificationItems";

        IStorageService storageService;

        public PushService()
        {
            storageService = SimpleIoc.Default.GetInstance<IStorageService>();
        }

        public void Register(RestAPIItem item)
        {
            if ( !IsRegistered(item)) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var notifications = (List<RestAPIItem>)storageService.GetSettingsEntry(notificationSettingsStr);
                notifications.Add(item);
                storageService.SetSettingsEntry(notificationSettingsStr, notifications);
            }
        }

        public void Unregister(RestAPIItem item)
        {
            if ( IsRegistered(item))  {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var notifications = (List<RestAPIItem>)storageService.GetSettingsEntry(notificationSettingsStr);
                notifications.RemoveAll(notification => notification.Url == item.Url);
                storageService.SetSettingsEntry(notificationSettingsStr, notifications);
            }
        }

        public bool IsRegistered(RestAPIItem item)
        {
            var notifications = (List<RestAPIItem>)storageService.GetSettingsEntry(notificationSettingsStr);

            return notifications.Exists(notification => notification.Url == item.Url);
        }
    }
}
