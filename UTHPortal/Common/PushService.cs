using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using UTHPortal.Models;

namespace UTHPortal.Common
{
    public class PushService : IPushService
    {
        IStorageService storageService;

        public PushService()
        {
            storageService = SimpleIoc.Default.GetInstance<IStorageService>();
        }

        public void Register(RestAPIItem item)
        {
            if (IsRegistered(item) == null) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
                activePushEvents.Add(new PushEvent(item.Url, item.Collection));
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        public void Unregister(RestAPIItem item)
        {
            PushEvent pushEvent;
            if ( (pushEvent = IsRegistered(item)) != null) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
                activePushEvents.Remove(pushEvent);
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        public PushEvent IsRegistered(RestAPIItem item)
        {
            var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");

            return activePushEvents.Find(activeEvent => activeEvent.Url == item.Url);
        }
    }
}
