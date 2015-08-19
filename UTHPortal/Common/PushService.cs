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

        public void RegisterUrl(string url)
        {
            PushEvent pushEvent;
            if ((pushEvent = IsRegistered(url)) == null) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
                activePushEvents.Add(pushEvent);
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        public void UnregisterUrl(string url)
        {
            PushEvent pushEvent;
            if ( (pushEvent = IsRegistered(url)) != null) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
                activePushEvents.Remove(pushEvent);
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        public PushEvent IsRegistered(string url)
        {
            var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
            url = getEventName(url);

            return activePushEvents.Find(activeEvent => activeEvent.Url == url);
        }

        private string getEventName(string url)
        {
            return url.Substring(RestAPI.baseUrl.Length).Replace('/', '.');
        }
    }
}
